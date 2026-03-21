# Kurumsal Proje Yönetimi: API Endpoint Tasarımı

Bu doküman, sistemdeki temel işlevleri karşılayacak RESTful API uç noktalarını açıklar.

---

## 1. Proje Yönetimi (`/api/projects`)

| Method | Endpoint | Açıklama |
| :--- | :--- | :--- |
| `GET` | `/api/projects` | Tüm projeleri listeler (opsiyonel filtreleme ile). |
| `POST` | `/api/projects` | Yeni bir proje oluşturur. |
| `GET` | `/api/projects/{id}` | Proje detaylarını (görevler ve ekip dahil) getirir. |
| `PUT` | `/api/projects/{id}` | Proje bilgilerini günceller. |
| `POST` | `/api/projects/{id}/complete` | Projeyi tamamlandı olarak işaretler (domain kuralları geçerlidir). |

---

## 2. Görev Yönetimi (`/api/projects/{id}/tasks`)

| Method | Endpoint | Açıklama |
| :--- | :--- | :--- |
| `GET` | `/api/projects/{id}/tasks` | Belirli bir projeye ait tüm görevleri listeler. |
| `POST` | `/api/projects/{id}/tasks` | Projeye yeni bir görev ekler. |
| `PATCH` | `/api/projects/{projectId}/tasks/{taskId}/status` | Görev durumunu günceller (To Do, In Progress, vb.). |
| `PATCH` | `/api/projects/{projectId}/tasks/{taskId}/assign` | Görevi bir personele atar. |

---

## 3. Personel ve Atama Yönetimi

| Method | Endpoint | Açıklama |
| :--- | :--- | :--- |
| `GET` | `/api/personnel` | Tüm personel listesini getirir. |
| `POST` | `/api/personnel` | Yeni bir personel kaydı açar. |
| `POST` | `/api/projects/{id}/assign` | Bir personeli projeye belirli bir rolle atar. |

---

## Tasarım İlkeleri

1. **Dikey Dilim (Vertical Slice)**: Her endpoint, kendi iş mantığını (Command/Query) `Features` klasörü altında izole bir şekilde barındıracaktır.
2. **Domain Kuralları**: Update/Create işlemlerinde Domain modelleri içindeki iş kuralları (Invariants) otomatik olarak işletilecektir.
3. **HTTP Status Codes**:
   - `200 OK`: Başarılı işlemler.
   - `201 Created`: Kayıt oluşturma işlemleri.
   - `400 Bad Request`: Domain kuralı ihlalleri veya geçersiz veri.
   - `404 Not Found`: Kayıt bulunamadığında.
