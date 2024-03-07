using Amazon.S3;
using Amazon.S3.Model;
using FinancialPlanning.Service.Services;
using Microsoft.Extensions.Configuration;

namespace Test.UnitTesting.Service.Services;

public class FileServiceTests
{
    private readonly IConfiguration _configuration;

    public FileServiceTests()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var appsettingsPath =
            Path.Combine(currentDirectory, @"..\..\..\..\FinancialPlanning.WebAPI\appsettings.json");

        var builder = new ConfigurationBuilder().AddJsonFile(appsettingsPath).Build();

        _configuration = builder;
    }

    [Theory]
    [InlineData("Template/Financial Plan_Template.xlsx")]
    [InlineData("Template/Monthly Expense Report_Template.xlsx")]
    public async Task GetFileAsyncTests(string key)
    {
        // Arrange
        var s3Client = new AmazonS3Client(_configuration["AWS:AccessKey"], _configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(_configuration["AWS:Region"]));
        var fileService = new FileService(s3Client, _configuration);

        var expectedRequest = new GetPreSignedUrlRequest
        {
            BucketName = _configuration["AWS:BucketName"],
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(15)
        };

        var expectedPreSignedUrl = await s3Client.GetPreSignedURLAsync(expectedRequest);

        //Act
        var result = await fileService.GetFileAsync(key);

        //Assert
        Assert.Equal(expectedPreSignedUrl, result);
    }

    [Theory]
    [InlineData("emptyfile.xlsx", 0)]
    [InlineData("wrongextension.txt", 0)]
    public void ValidateFileTests(string fileName, byte documentType)
    {
    }
}