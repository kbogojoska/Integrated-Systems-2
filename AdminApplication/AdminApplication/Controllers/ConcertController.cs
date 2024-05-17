using AdminApplication.Models.dto;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AdminApplication.Controllers
{
    public class ConcertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportUsers(IFormFile file)
        {
            string uploadPath = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using(FileStream fileStream = System.IO.File.Create(uploadPath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<ConcertDTO> concerts = getAllConcertsFromFile(file.FileName);

            HttpClient client = new HttpClient();
            string URL = "https://localhost:44341/api/Admin/ImportAllConcerts";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(concerts), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "Order");

            //return null;
        }

        private List<ConcertDTO> getAllConcertsFromFile(string fileName)
        {
            List<ConcertDTO> concerts = new List<ConcertDTO>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        concerts.Add(new Models.dto.ConcertDTO
                        {
                            ConcertName = reader.GetValue(0).ToString(),
                            ConcertDescription = reader.GetValue(1).ToString(),
                            ConcertImage = reader.GetValue(2).ToString(),
                            Rating = reader.GetValue(3).ToString()
                        });
                    }
                }
            }

            return concerts;
        }
    }
}
