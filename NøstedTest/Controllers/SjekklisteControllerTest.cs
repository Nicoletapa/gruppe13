
global using Xunit;

namespace  NøstedTest.Controllers;


public class SjekklisteControllerTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly SjekklisteController _controller;

    public SjekklisteControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new ApplicationDbContext(options);

        // Populate the in-memory database with test data if necessary

        _controller = new SjekklisteController(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
  

    private async Task<Guid> AddTestDataToContext()
    {
        var validId = Guid.NewGuid();

        // Create a test category
        var testKategori = new Kategori
        {
            KategoriID = 1,
            KategoriNavn = "Test Kategori",
        };
        _context.Kategori.Add(testKategori);

        // Create a test checkpoint
        var testSjekkpunkt = new Sjekkpunkt
        {
            SjekkpunktID = 1,
            SjekkpunktNavn = "Test Sjekkpunkt",
            KategoriID = testKategori.KategoriID, // Set the foreign key to the test category
        };
        _context.Sjekkpunkt.Add(testSjekkpunkt);

        // Create a test SjekklisteSjekkpunkt
        var testSjekklisteSjekkpunkt = new SjekklisteSjekkpunkt
        {
            SjekklisteID = validId,
            SjekkpunktID = testSjekkpunkt.SjekkpunktID,
            OrdreNr = 123, // Example order number
            Status = null,
            sjekkpunkt = testSjekkpunkt, // Associate the test checkpoint
        };
        _context.SjekklisteSjekkpunkt.Add(testSjekklisteSjekkpunkt);

        await _context.SaveChangesAsync();

        return validId;
    }

    [Fact]
    public async Task DetailsWithValidIdReturnsViewResult()
    {
        // Arrange
        var validId = await AddTestDataToContext(); // Await the result of AddTestDataToContext

        // Act
        var result = await _controller.Details(validId);

        // Assert
        Assert.NotNull(result);
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CreateSjekklisteSjekkpunktViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.Equal(validId, model.SjekklisteId);

        // Additional assertions to verify the data in model
        // For example, checking the contents of GroupedSjekkpunkter, etc.
        Assert.NotNull(model.GroupedSjekkpunkter);
        Assert.NotEmpty(model.GroupedSjekkpunkter); // Ensure there are groups of checkpoints
        // Further checks can be added based on what you expect in the GroupedSjekkpunkter
    }
  


    
    [Fact]
    public async Task EditWithValidSjekklisteIdReturnsViewResult()
    {
        // Arrange
        var validSjekklisteId = await AddTestDataToContext();

        // Act
        var result = await _controller.Edit(validSjekklisteId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CreateSjekklisteSjekkpunktViewModel>(viewResult.Model);
        Assert.NotNull(model);
        Assert.Equal(validSjekklisteId, model.SjekklisteId);
        // Additional assertions...
    }
    
    
    [Fact]
    public async Task Edit_PostWithValidModelUpdatesSjekklisteSjekkpunkt()
    {
        // Arrange
        var validSjekklisteId = await AddTestDataToContext();
        var viewModel = new CreateSjekklisteSjekkpunktViewModel
        {
            // Populate the viewModel with updated data
        };

        // Act
        var result = await _controller.Edit(viewModel);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Details", redirectToActionResult.ActionName);
        // Verify that the sjekklisteSjekkpunkt in _context has been updated
    }
   


}
