using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;
using System.IO;
using Newtonsoft.Json;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;
        private const String COMPENSATION_SEED_DATA_FILE = "resources/CompensationSeedData.json";


        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        /*
         * whether the date is set or not, use the CurrentDate as default value
         * Data persists by writing to json file
         * https://stackoverflow.com/questions/20626849/how-to-append-a-json-file-without-disturbing-the-formatting
         */
        public Compensation Create(Compensation compensation)
        {
            if(compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();

                //Reade all compensations and Deserialize
                //Add new compenstion list of comps
                //overwrite file with new comp list
                //more reliable then appending
                List<Compensation> compensations;
                using (FileStream fs = new FileStream(COMPENSATION_SEED_DATA_FILE, FileMode.Open))
                using (StreamReader sr = new StreamReader(fs))
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    compensations = serializer.Deserialize<List<Compensation>>(jr);
                }

                compensations.Add(compensation);

                // Update json data string
                var jsonData = JsonConvert.SerializeObject(compensations);
                System.IO.File.WriteAllText(COMPENSATION_SEED_DATA_FILE, jsonData);

            }

            return compensation;
        }

        /*
         * 
         */ 
        public Compensation GetById(int id)
        {
            if(id != null)
            {
                return _compensationRepository.GetById(id);
            }

            return null;
        }

    }
}
