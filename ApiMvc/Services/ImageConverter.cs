namespace ApiMvc.Services
{
    public class ImageConverter
    {
        public static string ConvertFromImageToBase(string imagePath)
        {
            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                string baseString = Convert.ToBase64String(imageBytes);

                return baseString;
            }
            catch (Exception error)
            {
                throw error;
            }
        }
    }
}
