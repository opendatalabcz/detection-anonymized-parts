using OpenCvSharp;

namespace DAPPTests
{
    public class DappAnalyzerTests
    {
        [Theory]
        [InlineData("../../../TestFiles/1.pdf")]
        public async Task AnalyzeTest(string path)
        {
            var pdf = await DappPDF.Create(File.ReadAllBytes(path), "1");
            var result = await PDFAnalyzer.AnalyzeAsync(pdf, true);
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
            Cv2.ImWrite("../../../TestFiles/pattern.png", pattern);
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
    }
}
