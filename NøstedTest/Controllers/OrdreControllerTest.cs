global using Xunit;


namespace NøstedTest.Controllers;

public class OrdreControllerTests
{
    private readonly OrdreController _controller;
    private readonly ApplicationDbContext _context;
    private readonly string _dbName;

    public OrdreControllerTests()
    {
        _dbName = $"OrdreDb_{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: _dbName)
            .Options;
        _context = new ApplicationDbContext(options);
        _controller = new OrdreController(_context);
    }

 
    public async Task ClearDatabase()
    {
        foreach (var entity in _context.Ordre1)
        {
            _context.Remove(entity);
        }
        await _context.SaveChangesAsync();
    }

    private OrdreViewModel CreateTestOrdreViewModel()
    {
        return new OrdreViewModel
        {
            OrdreNr = 1,
            Navn = "Test Navn",
            TelefonNr = 1234567890,
            Adresse = "Test Adresse",
            Type = "Test Type",
            Gjelder = "Test Gjelder",
            Epost = "test@example.com",
            Uke = 1,
            Registrert = DateTime.Now,
            Bestilling = "Test Bestilling",
            AvtaltLevering = DateTime.Now.AddDays(7),
            ProduktMotatt = DateTime.Now.AddDays(-1),
            AvtaltFerdigstillelse = DateTime.Now.AddDays(5),
            ServiceFerdig = null,
            AntallTimer = 10.55m,
            Status = 1
        };
    }

    [Fact]
    public async Task IndexReturnsAViewResultWithAListOfOrdres()
    {
        await ClearDatabase();
        // Arrange
       var testOrdre = CreateTestOrdreViewModel();

        _context.Ordre1.Add(testOrdre);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<OrdreCompletionViewModel>>(viewResult.Model);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task DetailsWithValidIdReturnsOrdre()
    {
        await ClearDatabase();
        // Arrange
        var testOrdre = CreateTestOrdreViewModel();
        _context.Ordre1.Add(testOrdre);
        _context.SaveChanges();

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<OrdreCompletionViewModel>(viewResult.Model);
        Assert.Equal(testOrdre.OrdreNr, model.Ordre.OrdreNr);
    }
   
    [Fact]
    public async Task DetailsWithInvalidIdReturnsNotFound()
    {
        await ClearDatabase();
        // Act
        var result = await _controller.Details(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task CreatePostValidModelAddsOrdre()
    {
        await ClearDatabase();
        // Arrange
        var ordre =  CreateTestOrdreViewModel();

        // Act
        var result = await _controller.Create(ordre);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(1, _context.Ordre1.Count());
    }

    [Fact]
    public async Task EditPostValidModelUpdatesOrdre()
    {
        await ClearDatabase(); 

        // Arrange
        var ordre =  CreateTestOrdreViewModel();
        _context.Ordre1.Add(ordre);
        _context.SaveChanges();

        ordre.AvtaltFerdigstillelse = null;
        
        // Act
        var result = await _controller.Edit(1, ordre);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        var ordreInDb = _context.Ordre1.Find(1);
        Assert.Equal(null,ordre.AvtaltFerdigstillelse);
    }

    [Fact]
    public async Task DeleteConfirmedDeletesOrdre()
    {
        await ClearDatabase();
        // Arrange
        var ordre =  CreateTestOrdreViewModel();
        _context.Ordre1.Add(ordre);
        _context.SaveChanges();

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Empty(_context.Ordre1);
    }



}
