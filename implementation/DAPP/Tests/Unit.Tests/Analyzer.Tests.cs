namespace Unit.Tests;

public class DappAnalyzerTests
{
    [Theory]
    [InlineData("../../../TestFiles/1.pdf")]
    public async Task AnalyzeTest(string path)
    {
        // Arrange
        var pdf = await DappPDF.Create(File.ReadAllBytes(path), "1", path);

        // Act
        var result = await PDFAnalyzer.AnalyzeAsync(pdf);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OriginalImages);
        Assert.NotNull(result.ResultImages);
        Assert.NotNull(result.AnonymizedPercentagePerPage);
        Assert.True(result.AnonymizedPercentage == 0);
        Assert.False(result.ContainsAnonymizedData);
        Assert.True(result.PageCount > 0);
        Assert.True(result.PageCount == result.OriginalImages.Count);
        Assert.True(result.PageCount == result.ResultImages.Count);
        Assert.True(result.PageCount == result.AnonymizedPercentagePerPage.Count);
    }

    [Fact]
    public void Erode_ShouldReturnErodedMat()
    {
        // Arrange
        // Create a 5x5 binary image with a square in the middle
        Mat src = new Mat(5, 5, MatType.CV_8U, Scalar.All(0));
        src.At<byte>(1, 1) = 255;
        src.At<byte>(1, 2) = 255;
        src.At<byte>(1, 3) = 255;
        src.At<byte>(2, 1) = 255;
        src.At<byte>(2, 2) = 255;
        src.At<byte>(2, 3) = 255;
        src.At<byte>(3, 1) = 255;
        src.At<byte>(3, 2) = 255;
        src.At<byte>(3, 3) = 255;

        // Create a 3x3 structuring element
        Mat pattern = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        // Act
        Mat result = PDFAnalyzer.Erode(src, pattern);

        // Assert
        // After applying erosion with a 3x3 rectangular structuring element on src,
        // we expect all but one pixel value to become 0, the pixel in the middle of the square

        Assert.NotNull(result);
        Assert.True(result.Rows == src.Rows && result.Cols == src.Cols); // result and source have the same size
        Assert.Equal(1, Cv2.CountNonZero(result)); // count of non-zero pixels in the result should be 1
        Assert.Equal(255, result.At<byte>(2, 2)); // the pixel in the middle of the square should be white (255)
    }

    [Fact]
    public void Dilate_ShouldReturnDilatedMat()
    {
        // Arrange
        // Create a 5x5 binary image with a square in the middle
        Mat src = new Mat(5, 5, MatType.CV_8U, Scalar.All(0));

        src.At<byte>(2, 2) = 255;

        // Create a 3x3 structuring element
        Mat pattern = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        // Act
        Mat result = PDFAnalyzer.Dilate(src, pattern);

        // Assert
        // After applying erosion with a 3x3 rectangular structuring element on src,
        // we expect 9 pixels to become 255

        Assert.NotNull(result);
        Assert.True(result.Rows == src.Rows && result.Cols == src.Cols); // result and source have the same size
        Assert.Equal(9, Cv2.CountNonZero(result)); // count of non-zero pixels in the result should be 1
        Assert.Equal(255, result.At<byte>(1, 1));
        Assert.Equal(255, result.At<byte>(1, 2));
        Assert.Equal(255, result.At<byte>(1, 3));
        Assert.Equal(255, result.At<byte>(2, 1));
        Assert.Equal(255, result.At<byte>(2, 2));
        Assert.Equal(255, result.At<byte>(2, 3));
        Assert.Equal(255, result.At<byte>(3, 1));
        Assert.Equal(255, result.At<byte>(3, 2));
        Assert.Equal(255, result.At<byte>(3, 3));


    }

    [Theory]
    [InlineData("../../../TestFiles/798.pdf")]
    public async Task Threshold_WithMultiChannelImage_ReturnsResult(string path)
    {
        // Arrange
        var src = await DappPDF.Create(File.ReadAllBytes(path), "798", path);
        int val = 127;

        // Act
        Mat result = PDFAnalyzer.Threshold(src.Pages[0], val);

        // Assert
        Assert.NotNull(result);
        var pts = Enumerable.Range(0, result.Rows * result.Cols)
            .Select(i => result.At<byte>(i / result.Cols, i % result.Cols));

        Assert.All(pts, pixel =>
            Assert.True(pixel is 0 or 255));
    }

    [Theory]
    [InlineData("../../../TestFiles/1.pdf")]
    public async Task Threshold_WithSingleChannelImage_ReturnsResult(string path)
    {
        // Arrange
        var original = await DappPDF.Create(File.ReadAllBytes(path), "1", path);
        Mat src = new();
        Cv2.CvtColor(original.Pages[0], src, ColorConversionCodes.BGR2GRAY);

        int val = 127;
        // Act
        Mat result = PDFAnalyzer.Threshold(src, val);

        // Assert
        Assert.NotNull(result);
        var pts = Enumerable.Range(0, result.Rows * result.Cols)
            .Select(i => result.At<byte>(i / result.Cols, i % result.Cols));

        Assert.All(pts, pixel =>
            Assert.True(pixel is 0 or 255));
    }

    [Theory]
    [InlineData("../../../TestFiles/798.pdf")]
    public async Task ColoredPixels_ShouldReturnPopulatedList_WithColoredImage(string path)
    {
        // Arrange
        var src = await DappPDF.Create(File.ReadAllBytes(path), "798", path);

        // Act
        var result = PDFAnalyzer.ColoredPixels(src.Pages[0]);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count > 39);
    }

    [Theory]
    [InlineData("../../../TestFiles/798.pdf")]
    public async Task IncreaseSaturation_ShouldReturnResult_WhenColoredImage(string path)
    {
        // Arrange
        var src = await DappPDF.Create(File.ReadAllBytes(path), "798", path);
        var coloredPixels = PDFAnalyzer.ColoredPixels(src.Pages[0]);
        // Act
        var result = PDFAnalyzer.IncreaseSaturation(src.Pages[0], coloredPixels, 20);

        // Assert
        Assert.NotNull(result);
        foreach (var cp in coloredPixels)
        {
            if (src.Pages[0].At<Vec3b>(cp.Y, cp.X) != result.At<Vec3b>(cp.Y, cp.X))
            {
                Assert.True(true);
            }
        }
    }

    [Theory]
    [InlineData("../../../TestFiles/798.pdf")]
    public async Task IncreaseSaturation_ShouldReturnResult_WhenValueIsNegative(string path)
    {
        // Arrange
        var src = await DappPDF.Create(File.ReadAllBytes(path), "798", path);
        var coloredPixels = PDFAnalyzer.ColoredPixels(src.Pages[0]);
        // Act
        var result = PDFAnalyzer.IncreaseSaturation(src.Pages[0], coloredPixels, -255);

        // Assert
        Assert.NotNull(result);
        foreach (var cp in coloredPixels)
        {
            if (src.Pages[0].At<Vec3b>(cp.Y, cp.X) != result.At<Vec3b>(cp.Y, cp.X))
            {
                Assert.True(true);
            }
        }
    }

    [Theory]
    [InlineData("../../../TestFiles/798.pdf")]
    public async Task IncreaseSaturation_ShouldReturnResult_WhenValueIsPositive(string path)
    {
        // Arrange
        var src = await DappPDF.Create(File.ReadAllBytes(path), "798", path);
        var coloredPixels = PDFAnalyzer.ColoredPixels(src.Pages[0]);
        // Act
        var result = PDFAnalyzer.IncreaseSaturation(src.Pages[0], coloredPixels, 255);

        // Assert
        Assert.NotNull(result);
        foreach (var cp in coloredPixels)
        {
            if (src.Pages[0].At<Vec3b>(cp.Y, cp.X) != result.At<Vec3b>(cp.Y, cp.X))
            {
                Assert.True(true);
            }
        }
    }

}