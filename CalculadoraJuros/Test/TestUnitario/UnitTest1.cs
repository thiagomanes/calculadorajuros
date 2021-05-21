using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestUnitario
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:56167/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            decimal taxa = 0M;

            HttpResponseMessage response = await client.GetAsync("calculajuros?valorInicial=100&meses=5");
            if (response.IsSuccessStatusCode)
            {
                var taxaStr = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(taxaStr))
                    taxa = decimal.Parse(taxaStr);

            }

            Assert.AreEqual(105.10M, taxa);
        }
    }
}
