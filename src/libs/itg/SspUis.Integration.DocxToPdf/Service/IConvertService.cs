using StatusGeneric;

namespace SspUis.Integration.DocxToPdf;

public interface IConvertService : IStatusGeneric
{
    /// <summary>
    /// Word shablonga malumotlarni yozib pdfga o'tkazish uchun.
    /// </summary>
    /// <param name="file">Word shablon file.</param>
    /// <param name="data">Yoziladigan ma'lumotlar obyekti. </param>
    /// <returns>bu byte arrayni return File(bytes,"application/pdf"); qilib frontga qaytarsa bo'ladi.</returns>
    ValueTask<byte[]> DocxToPdfAsync(MemoryStream file, object data);
}
