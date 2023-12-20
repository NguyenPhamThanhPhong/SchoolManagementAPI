using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
#pragma warning disable CS8618

namespace SchoolManagementAPI.Services.Configs
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string StudentCollectionName { get; set; }
        public string SchoolClassCollectionName { get; set; }
        public string SubjectCollectionName { get; set; }
        public string LecturerCollectionName { get; set; }
        public string AdminCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }


        public IMongoClient MongoClient { get; set; }
        public IMongoDatabase  MongoDatabase { get; set; }
        public IMongoCollection<SchoolClass> SchoolClassCollection { get; set; }
        public IMongoCollection<Subject> SubjectCollection { get; set; }

        public IMongoCollection<Admin> AdminCollection { get; set; }
        public IMongoCollection<Lecturer> LecturerCollection { get; set; }
        public IMongoCollection<Student> StudentCollection { get; set; }

        public IMongoCollection<Category> CategoryCollection { get; set; }

        public void SetUpDatabase()
        {
            InstantiateCollections();
        }
        private void InstantiateCollections()
        {
            MongoClient = new MongoClient(ConnectionString);
            MongoDatabase = MongoClient.GetDatabase(DatabaseName);

            SchoolClassCollection = MongoDatabase.GetCollection<SchoolClass>(SchoolClassCollectionName);
            
            SubjectCollection = MongoDatabase.GetCollection<Subject>(SubjectCollectionName);

            AdminCollection = MongoDatabase.GetCollection<Admin>(AdminCollectionName);

            LecturerCollection = MongoDatabase.GetCollection<Lecturer>(LecturerCollectionName);
            StudentCollection = MongoDatabase.GetCollection<Student>(StudentCollectionName);
                        

            CategoryCollection = MongoDatabase.GetCollection<Category>(CategoryCollectionName);
        }

        //private void CreateUniqueIndex()
        //{
        //    var indexKeysDefinition = Builders<User>.IndexKeys.Ascending(User.GetFieldName(u=>u.AuthenticationInfo.Username));
        //    var indexOptions = new CreateIndexOptions { Unique = true };

        //    UserCollection.Indexes.CreateOne(new CreateIndexModel<User>(indexKeysDefinition, indexOptions));
        //}
    }
}
