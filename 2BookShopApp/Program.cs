using _2BookShopApp;
using(var dbContext = new BookShopDbContext())
{
    dbContext.Database.EnsureCreated();
    var bookStore = new BookStore(dbContext);
    bookStore.Login();
}