# PharmaTrack

بنيت المشروع ده كـ backend system لإدارة الصيدليات، الفكرة الأساسية إن الصيدليات عندها مشكلة كبيرة مع الأدوية القريبة من انتهاء الصلاحية — بتخسرها أو بترميها. فعملت نظام يراقب الجرد تلقائياً ويبعت تنبيهات، وكمان سوق داخلي تقدر من خلاله الصيدلية تبيع الأدوية دي لصيدليات تانية بخصم بدل ما ترميها.

المشروع مبني على **Microservices Architecture** بـ ASP.NET Core 10، وكل service ليه مسؤولية واحدة بس.

---

## إيه اللي بيعمله النظام؟

- صيدلية بتسجل نفسها وبتضيف جردها (يدوي أو بـ QR scan)
- كل يوم فيه Worker بيتشغل في الخلفية ويجيب كل الأدوية اللي هتنتهي خلال 90 يوم
- بيبعت تنبيه للصيدلي على الـ dashboard في real-time وكمان على الإيميل
- الصيدلي يقدر يرفع الدواء ده على الـ Marketplace بخصم 20% تلقائي
- صيدلية تانية محتاجة الدواء ده تقدر تطلبه والنظام يعمل match بينهم

---

## الـ Services

المشروع فيه 7 services بتتكلم مع بعض عن طريق API Gateway:

**API Gateway** — بوابة الدخول الوحيدة للنظام. بيتحقق من الـ JWT token قبل ما يوصل أي request للـ services، وبيعمل rate limiting عشان يحمي من الـ abuse.

**Identity Service** — مسؤول عن التسجيل، الـ login، وإصدار JWT tokens. الـ passwords بتتخزن بـ BCrypt والـ tokens بتنتهي بعد 60 دقيقة مع إمكانية التجديد بـ refresh token.

**Pharmacy Service** — إدارة الصيدليات والفروع. كل صيدلية لما بتتسجل بتبقى status = PendingApproval والـ SuperAdmin هو اللي بيفعّلها.

**Inventory Service** — إدارة الجرد الكامل. كل منتج ممكن يكون فيه أكتر من batch، وكل batch بيتعمله QR code تلقائي لما بتضيفه.

**Notification Service** — بيبعت التنبيهات على 4 channels: الـ dashboard بـ SignalR، إيميل بـ SendGrid، SMS بـ Twilio، وـ push notifications بـ Firebase. كل notification بيتسجل في قاعدة البيانات مع حالته (sent/failed).

**Marketplace Service** — صيدلية بتعرض دواء، صيدلية تانية بتطلب، والنظام بيعمل match بينهم. الخصم ثابت 20% على كل اللي بيتعرض.

**Expiry Tracker Worker** — مش API، ده background service بيشتغل كل 24 ساعة. بيقرأ من قاعدة بيانات الـ Inventory مباشرة ويبعت alerts للـ Notification Service.

---

## التقنيات المستخدمة

- **ASP.NET Core 10 / .NET 10**
- **PostgreSQL** — قاعدة بيانات واحدة بـ schemas منفصلة لكل service
- **Entity Framework Core** مع Npgsql
- **CQRS + MediatR** — كل operation عبارة عن Command أو Query
- **FluentValidation** — الـ validation بيحصل في pipeline قبل ما يوصل للـ handler
- **YARP** — الـ Reverse Proxy بتاع الـ Gateway
- **SignalR** — الـ real-time notifications
- **Serilog** — الـ logging
- **Docker + Docker Compose** — لتشغيل كل حاجة مع بعض بأمر واحد
- **QRCoder** — لتوليد الـ QR codes
- **SendGrid / Twilio / Firebase** — للـ notifications

---

## بنية المشروع

```
PharmaTrack/
├── .env                          # المتغيرات البيئية — متتعملش push
├── docker-compose.yml
└── services/
    ├── ApiGateway/
    ├── IdentityService/
    ├── PharmacyService/
    ├── InventoryService/
    ├── NotificationService/
    ├── MarketplaceService/
    └── ExpiryTrackerService/
```

كل service جوه نفسه:
```
ServiceName/
├── ServiceName.API/          → Controllers, Middleware, Program.cs
├── ServiceName.Application/  → Commands, Queries, DTOs, Interfaces
├── ServiceName.Domain/       → Entities, Enums, Exceptions
└── ServiceName.Infrastructure/→ DbContext, Repositories, External services
```

---

## تشغيل المشروع

### المتطلبات
- Docker Desktop

### الخطوات

**1. Clone المشروع**
```bash
git clone https://github.com/islamallam962-stack/pharmatrack.git
cd pharmatrack
```

**2. أنشئ ملف الـ `.env`**

في root المشروع انسخ الملف التالي واحط قيمك فيه:
```bash
cp .env.example .env
```

افتح الـ `.env` وغيّر القيم دي على الأقل:
```env
POSTGRES_PASSWORD=اختار_باسورد_قوي
JWT_SECRET=سلسلة_عشوائية_32_حرف_على_الأقل
SENDGRID_API_KEY=مفتاح_الـ_SendGrid_بتاعك
```

**3. شغّل كل حاجة**
```bash
docker-compose up --build
```

خلي الأمر ده يخلص، هيعمل build لكل الـ services ويشغّلهم، وقاعدة البيانات هتتعمل تلقائياً.

**4. جرّب النظام**

| Service | URL |
|---|---|
| API Gateway (كل الطلبات هنا) | http://localhost:5000 |
| Identity Service | http://localhost:5001/scalar |
| Pharmacy Service | http://localhost:5002/scalar |
| Inventory Service | http://localhost:5003/scalar |
| Notification Service | http://localhost:5004/scalar |
| Marketplace Service | http://localhost:5005/scalar |

---

## التشغيل بدون Docker (للـ development)

لو عايز تشغّل service بمفرده:

```bash
cd services/IdentityService/IdentityService.API
dotnet run
```

لكن محتاج PostgreSQL شغّال locally على port 5432 الأول.

---

## الـ Authentication

كل الطلبات (غير التسجيل والـ login) محتاجة JWT token.

**1. سجّل يوزر**
```
POST http://localhost:5000/api/auth/register
{
  "fullName": "اسمك",
  "email": "email@example.com",
  "password": "Password123",
  "role": 2
}
```

الـ roles:
- `1` = SuperAdmin
- `2` = PharmacyAdmin
- `3` = Pharmacist

**2. اعمل login وجيب الـ token**
```
POST http://localhost:5000/api/auth/login
```
هيرجعلك `accessToken` — حطه في كل الطلبات الجاية كـ:
```
Authorization: Bearer <accessToken>
```

الـ token بيخلص بعد 60 دقيقة، جدّده عن طريق:
```
POST http://localhost:5000/api/auth/refresh
```

---

## الـ Endpoints الأساسية

كل الطلبات بتروح على الـ Gateway — `http://localhost:5000`

### الصيدليات
```
POST   /api/pharmacies              → أنشئ صيدلية
GET    /api/pharmacies/{id}         → تفاصيل صيدلية
GET    /api/pharmacies              → كل الصيدليات (SuperAdmin بس)
PUT    /api/pharmacies/{id}         → تعديل
PATCH  /api/pharmacies/{id}/status  → تفعيل أو إيقاف (SuperAdmin)
```

### الجرد
```
POST   /api/inventory/manual              → أضف منتج يدوي
POST   /api/inventory/scan                → scan QR
GET    /api/inventory/{pharmacyId}        → كل جرد صيدلية
PATCH  /api/inventory/batch/{id}/stock    → تحديث الكمية
DELETE /api/inventory/product/{id}        → حذف منتج
```

### الـ Marketplace
```
POST  /api/marketplace/listings             → أضف دواء للبيع
GET   /api/marketplace/listings             → شوف المتاح
POST  /api/marketplace/requests             → اطلب دواء
POST  /api/marketplace/match                → ربط عرض بطلب
```

### الـ Real-time Notifications
الـ frontend بيعمل connect على:
```
ws://localhost:5000/hubs/notifications?access_token=<JWT>
```
وبيـ join الـ group بتاعته:
```js
connection.invoke("JoinPharmacyGroup", pharmacyId)
```
وبيستقبل الـ alerts على event اسمه `ReceiveAlert`.

---

## ملاحظات مهمة

**QR Code:** الـ scan endpoint بيستقبل الـ decoded string من الـ QR مش الصورة نفسها. الـ decode بيحصل على الـ mobile/frontend. الـ format:
```
PHARMA|{productId}|{batchId}|{batchNumber}
```

**الصيدلية الجديدة:** أي صيدلية بتتسجل حالتها بتبقى `PendingApproval`. الـ SuperAdmin هو اللي بيعمل activate بـ:
```
PATCH /api/pharmacies/{id}/status
body: "activate"
```

**الـ Expiry Tracker:** بيشتغل أول ما الـ Docker يقوم، وبعدين كل 24 ساعة. بيشوف كل الـ batches اللي هتنتهي خلال 90 يوم وبيبعت alert مرة واحدة في اليوم لكل batch.

---

## المتغيرات البيئية

| المتغير | الوصف |
|---|---|
| `POSTGRES_DB` | اسم قاعدة البيانات |
| `POSTGRES_USER` | اليوزر |
| `POSTGRES_PASSWORD` | الباسورد |
| `JWT_SECRET` | المفتاح السري للـ JWT (32 حرف على الأقل) |
| `JWT_ISSUER` | اسم المصدر (PharmaTrack) |
| `JWT_AUDIENCE` | الجمهور المستهدف (PharmaTrackClients) |
| `SENDGRID_API_KEY` | مفتاح SendGrid للإيميلات |
| `TWILIO_ACCOUNT_SID` | حساب Twilio للـ SMS |
| `TWILIO_AUTH_TOKEN` | Token الـ Twilio |
| `TWILIO_FROM_PHONE` | رقم الإرسال على Twilio |
| `FIREBASE_CREDENTIAL_PATH` | مسار ملف credentials الـ Firebase |
