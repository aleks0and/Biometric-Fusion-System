using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Model
{
    class Identification
    {
        List<Person> persons;
        PersonRepository personRepository;

        public Identification(DbConnection dbConnection)
        {
            persons = new List<Person>();
            personRepository = new PersonRepository(dbConnection);
        }

        public void LoadPersonsList()
        {
            if(persons.Count == 0)
            {
                persons = personRepository.SelectPersons();
            }
        }

        public void IdentifySpeech()
        {

        }
    }
}
