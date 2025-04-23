var builder = WebApplication.CreateBuilder(args);

// ✅ Razor View + JSON binding
builder.Services.AddControllersWithViews();

// ✅ Session için destek
builder.Services.AddSession();

// ✅ HTTP istekleri için (login gibi)
builder.Services.AddHttpClient();

// ✅ TempData için Cookie-based TempData Provider
builder.Services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider, Microsoft.AspNetCore.Mvc.ViewFeatures.CookieTempDataProvider>();

// ✅ Hata mesajları için TempData'ya erişim
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// ✅ Error page (Production)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ✅ HTTPS + Static dosyalar (CSS, JS, img)
app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Routing
app.UseRouting();

// ✅ Session mutlaka authorization'dan önce gelmeli
app.UseSession();

// ✅ Yetki kontrolü (JWT değilse form auth için kullanılır)
app.UseAuthorization();

// ✅ Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
