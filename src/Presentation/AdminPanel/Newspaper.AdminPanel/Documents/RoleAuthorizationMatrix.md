# Admin Panel - Rol Yetkilendirme Matrisi

## **Rol Hiyerarşisi**

### **1. SuperAdmin (Süper Yönetici)**
- **Açıklama**: Sistemin tam kontrolüne sahip en üst seviye rol
- **Yetkiler**: Tüm sistem yetkileri
- **Kullanıcı Sayısı**: 1-2 kişi (Sistem yöneticileri)

### **2. Admin (Yönetici)**
- **Açıklama**: Genel sistem yönetimi ve içerik kontrolü
- **Yetkiler**: Çoğu sistem yetkisi (SuperAdmin hariç)
- **Kullanıcı Sayısı**: 3-5 kişi (Site yöneticileri)

### **3. Editor (Editör)**
- **Açıklama**: İçerik yönetimi ve yayınlama
- **Yetkiler**: İçerik oluşturma, düzenleme, yayınlama
- **Kullanıcı Sayısı**: 5-10 kişi (İçerik editörleri)

### **4. Author (Yazar)**
- **Açıklama**: Makale yazma ve kendi içeriklerini yönetme
- **Yetkiler**: Kendi makalelerini yazma ve düzenleme
- **Kullanıcı Sayısı**: 10-20 kişi (İçerik yazarları)

### **5. Moderator (Moderatör)**
- **Açıklama**: Yorum moderasyonu ve kullanıcı yönetimi
- **Yetkiler**: Yorum onaylama, kullanıcı yönetimi
- **Kullanıcı Sayısı**: 3-8 kişi (Topluluk moderatörleri)

### **6. Viewer (Görüntüleyici)**
- **Açıklama**: Sadece içerik görüntüleme
- **Yetkiler**: Sadece okuma yetkisi
- **Kullanıcı Sayısı**: Sınırsız (Ziyaretçiler)

---

## **Sayfa Yetkilendirme Matrisi**

### **Dashboard (Ana Sayfa)**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm istatistikleri görüntüleme, sistem durumu |
| Admin | ✅ Tam Erişim | Tüm istatistikleri görüntüleme |
| Editor | ✅ Tam Erişim | İçerik istatistikleri, kendi verileri |
| Author | ✅ Tam Erişim | Kendi makale istatistikleri |
| Moderator | ✅ Tam Erişim | Yorum istatistikleri, kullanıcı aktiviteleri |
| Viewer | ❌ Erişim Yok | - |

### **Kullanıcı Yönetimi**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm kullanıcıları yönetme, rol atama, silme |
| Admin | ✅ Tam Erişim | Kullanıcı yönetimi (SuperAdmin hariç) |
| Editor | ✅ Sınırlı Erişim | Kendi profilini düzenleme |
| Author | ✅ Sınırlı Erişim | Kendi profilini düzenleme |
| Moderator | ✅ Sınırlı Erişim | Kullanıcı listesi görüntüleme, profil düzenleme |
| Viewer | ❌ Erişim Yok | - |

### **Rol Yönetimi**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Rol oluşturma, düzenleme, silme, atama |
| Admin | ✅ Sınırlı Erişim | Rol görüntüleme, kullanıcılara rol atama |
| Editor | ❌ Erişim Yok | - |
| Author | ❌ Erişim Yok | - |
| Moderator | ❌ Erişim Yok | - |
| Viewer | ❌ Erişim Yok | - |

### **Kategori Yönetimi**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Kategori oluşturma, düzenleme, silme |
| Admin | ✅ Tam Erişim | Kategori oluşturma, düzenleme, silme |
| Editor | ✅ Tam Erişim | Kategori oluşturma, düzenleme |
| Author | ✅ Sınırlı Erişim | Kategori listesi görüntüleme |
| Moderator | ✅ Sınırlı Erişim | Kategori listesi görüntüleme |
| Viewer | ❌ Erişim Yok | - |

### **Makale Yönetimi**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm makaleleri yönetme, yayınlama, silme |
| Admin | ✅ Tam Erişim | Tüm makaleleri yönetme, yayınlama, silme |
| Editor | ✅ Tam Erişim | Makale oluşturma, düzenleme, yayınlama |
| Author | ✅ Sınırlı Erişim | Kendi makalelerini oluşturma, düzenleme |
| Moderator | ✅ Sınırlı Erişim | Makale listesi görüntüleme |
| Viewer | ❌ Erişim Yok | - |

### **Etiket Yönetimi**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Etiket oluşturma, düzenleme, silme |
| Admin | ✅ Tam Erişim | Etiket oluşturma, düzenleme, silme |
| Editor | ✅ Tam Erişim | Etiket oluşturma, düzenleme |
| Author | ✅ Sınırlı Erişim | Etiket listesi görüntüleme |
| Moderator | ✅ Sınırlı Erişim | Etiket listesi görüntüleme |
| Viewer | ❌ Erişim Yok | - |

### **Yorum Yönetimi**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm yorumları yönetme, onaylama, silme |
| Admin | ✅ Tam Erişim | Tüm yorumları yönetme, onaylama, silme |
| Editor | ✅ Sınırlı Erişim | Kendi makalelerindeki yorumları yönetme |
| Author | ✅ Sınırlı Erişim | Kendi makalelerindeki yorumları görüntüleme |
| Moderator | ✅ Tam Erişim | Yorum onaylama, reddetme, silme |
| Viewer | ❌ Erişim Yok | - |

### **Raporlar**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm raporları görüntüleme, export |
| Admin | ✅ Tam Erişim | Tüm raporları görüntüleme, export |
| Editor | ✅ Sınırlı Erişim | İçerik raporları, kendi istatistikleri |
| Author | ✅ Sınırlı Erişim | Kendi makale raporları |
| Moderator | ✅ Sınırlı Erişim | Yorum raporları, kullanıcı aktiviteleri |
| Viewer | ❌ Erişim Yok | - |

### **Sistem Ayarları**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm sistem ayarlarını değiştirme |
| Admin | ✅ Sınırlı Erişim | Genel site ayarları |
| Editor | ❌ Erişim Yok | - |
| Author | ❌ Erişim Yok | - |
| Moderator | ❌ Erişim Yok | - |
| Viewer | ❌ Erişim Yok | - |

### **Sistem Logları**
| Rol | Erişim | İşlevler |
|-----|--------|----------|
| SuperAdmin | ✅ Tam Erişim | Tüm sistem loglarını görüntüleme |
| Admin | ✅ Sınırlı Erişim | Genel sistem logları |
| Editor | ❌ Erişim Yok | - |
| Author | ❌ Erişim Yok | - |
| Moderator | ❌ Erişim Yok | - |
| Viewer | ❌ Erişim Yok | - |

---

## **Detaylı Yetki Açıklamaları**

### **SuperAdmin Yetkileri**
- ✅ Tüm kullanıcıları yönetme (oluşturma, düzenleme, silme)
- ✅ Tüm rolleri yönetme (oluşturma, düzenleme, silme)
- ✅ Sistem ayarlarını değiştirme
- ✅ Sistem loglarını görüntüleme
- ✅ Tüm içerikleri yönetme
- ✅ Yedekleme ve geri yükleme
- ✅ Sistem bakım modu

### **Admin Yetkileri**
- ✅ Kullanıcı yönetimi (SuperAdmin hariç)
- ✅ İçerik yönetimi
- ✅ Kategori ve etiket yönetimi
- ✅ Yorum moderasyonu
- ✅ Rapor görüntüleme
- ✅ Genel site ayarları

### **Editor Yetkileri**
- ✅ Makale oluşturma ve düzenleme
- ✅ Kategori oluşturma ve düzenleme
- ✅ Etiket oluşturma ve düzenleme
- ✅ Kendi makalelerindeki yorumları yönetme
- ✅ İçerik raporları görüntüleme

### **Author Yetkileri**
- ✅ Kendi makalelerini oluşturma ve düzenleme
- ✅ Kategori ve etiket listesi görüntüleme
- ✅ Kendi makalelerindeki yorumları görüntüleme
- ✅ Kendi makale raporları

### **Moderator Yetkileri**
- ✅ Yorum onaylama ve reddetme
- ✅ Kullanıcı listesi görüntüleme
- ✅ Kullanıcı profil düzenleme
- ✅ Yorum raporları görüntüleme
- ✅ Kullanıcı aktivite raporları

### **Viewer Yetkileri**
- ❌ Admin paneline erişim yok
- ✅ Sadece public web sitesini görüntüleme

---

## **Güvenlik Kuralları**

### **1. Rol Hiyerarşisi**
- Üst rol, alt rolün tüm yetkilerine sahiptir
- Alt rol, üst rolün yetkilerine sahip olamaz
- Her kullanıcı sadece bir role sahip olabilir

### **2. Erişim Kontrolü**
- Tüm sayfalar rol kontrolü yapmalı
- Yetkisiz erişim denemeleri loglanmalı
- Session timeout uygulanmalı

### **3. Veri Güvenliği**
- Kullanıcılar sadece kendi verilerini düzenleyebilir
- Silme işlemleri onay gerektirmeli
- Kritik işlemler loglanmalı

### **4. Audit Trail**
- Tüm değişiklikler loglanmalı
- Kim, ne zaman, neyi değiştirdi kaydedilmeli
- Loglar silinememeli

---

## **Uygulama Kuralları**

### **1. Controller Seviyesi**
```csharp
[Authorize(Roles = "SuperAdmin,Admin")]
public class UsersController : Controller
```

### **2. Action Seviyesi**
```csharp
[Authorize(Roles = "SuperAdmin")]
public async Task<IActionResult> DeleteUser(Guid id)
```

### **3. View Seviyesi**
```html
@if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
{
    <button class="btn btn-danger">Sil</button>
}
```

### **4. JavaScript Seviyesi**
```javascript
if (userRoles.includes('SuperAdmin') || userRoles.includes('Admin')) {
    showDeleteButton();
}
```

---

## **Varsayılan Roller ve Kullanıcılar**

### **Seed Data**
- **SuperAdmin**: admin@newspaper.com (Super123!)
- **Admin**: manager@newspaper.com (Manager123!)
- **Editor**: editor@newspaper.com (Editor123!)
- **Author**: author@newspaper.com (Author123!)
- **Moderator**: moderator@newspaper.com (Moderator123!)

### **Rol Oluşturma Sırası**
1. SuperAdmin
2. Admin
3. Editor
4. Author
5. Moderator
6. Viewer (varsayılan rol) 