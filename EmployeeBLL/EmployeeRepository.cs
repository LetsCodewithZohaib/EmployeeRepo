using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeBLL
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string token = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

        public Root GetAll(string criteria="")
        {
            RestClient client = new RestClient("https://gorest.co.in/public-api/");
            var request = new RestRequest("users"+ criteria.Trim(), Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode==System.Net.HttpStatusCode.OK)
            {               
                var bsObj = JsonConvert.DeserializeObject<Root>(content);
                return bsObj;
            }
            else
            {
                return null;
            }           
        }

        public bool AddEmployee(Datum objEmployee)
        {
            var dataEmployee = JsonConvert.SerializeObject(objEmployee);
            RestClient client = new RestClient("https://gorest.co.in/public-api/");
            var request = new RestRequest("users", Method.POST);
            request.AddHeader("Authorization", "Bearer "+ token);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(dataEmployee);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode== System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateEmployee(Datum objEmployee)
        {
            var dataEmployee = JsonConvert.SerializeObject(objEmployee);
            RestClient client = new RestClient("https://gorest.co.in/public-api/");
            var request = new RestRequest("users/"+objEmployee.id, Method.PUT);          
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(dataEmployee);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Datum> Search(string criteria)
        {
            RestClient client = new RestClient("https://gorest.co.in/public-api/");
            var request = new RestRequest("users"+ criteria.Trim(), Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var bsObj = JsonConvert.DeserializeObject<Root>(content);
                return bsObj.data.ToList();
            }
            else
            {
                return null;
            }
        }

        public bool DeleteEmployee(int id)
        {            
            RestClient client = new RestClient("https://gorest.co.in/public-api/");
            var request = new RestRequest("users/" + id, Method.DELETE);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + token);           
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}
