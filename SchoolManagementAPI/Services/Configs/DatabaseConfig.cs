using MongoDB.Driver;
using SchoolManagementAPI.Models.Entities;
#pragma warning disable CS8618

namespace SchoolManagementAPI.Services.Configs
{
    public class DatabaseConfig
    {
        public string MyConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string StudentCollectionName { get; set; }
        public string SchoolClassCollectionName { get; set; }
        public string SubjectCollectionName { get; set; }
        public string LecturerCollectionName { get; set; }
        public string AdminCollectionName { get; set; }
        public string SemesterCollectionName { get; set; }
        public string FacultyCollectionName { get; set; }


        public IMongoClient MongoClient { get; set; }
        public IMongoDatabase  MongoDatabase { get; set; }
        public IMongoCollection<SchoolClass> SchoolClassCollection { get; set; }
        public IMongoCollection<Subject> SubjectCollection { get; set; }

        public IMongoCollection<Admin> AdminCollection { get; set; }
        public IMongoCollection<Lecturer> LecturerCollection { get; set; }
        public IMongoCollection<Student> StudentCollection { get; set; }

        public IMongoCollection<Semester> SemesterCollection { get; set; }
        public IMongoCollection<Faculty> FacultyCollection { get; set; }

        public void SetUpDatabase()
        {
            InstantiateCollections();
            CreateUniqueIndex();
        }
        private void InstantiateCollections()
        {
            MongoClient = new MongoClient(MyConnectionString);
            MongoDatabase = MongoClient.GetDatabase(DatabaseName);

            SchoolClassCollection = MongoDatabase.GetCollection<SchoolClass>(SchoolClassCollectionName);
            
            SubjectCollection = MongoDatabase.GetCollection<Subject>(SubjectCollectionName);

            AdminCollection = MongoDatabase.GetCollection<Admin>(AdminCollectionName);

            LecturerCollection = MongoDatabase.GetCollection<Lecturer>(LecturerCollectionName);
            StudentCollection = MongoDatabase.GetCollection<Student>(StudentCollectionName);
                        

            SemesterCollection = MongoDatabase.GetCollection<Semester>(SemesterCollectionName);
            FacultyCollection = MongoDatabase.GetCollection<Faculty>(FacultyCollectionName);
        }

        private void CreateUniqueIndex()
        {
            var studentIndexKeysDefinition = Builders<Student>
                .IndexKeys.Ascending(Student.GetFieldName(u => u.Username));
            var lecturerIndexKeysDefinition = Builders<Lecturer>
                .IndexKeys.Ascending(Lecturer.GetFieldName(u => u.Username));
            var adminIndexKeysDefinition = Builders<Admin>
                .IndexKeys.Ascending(Admin.GetFieldName(u => u.Username));
            var indexOptions = new CreateIndexOptions { Unique = true };

            StudentCollection.Indexes.CreateOne(new CreateIndexModel<Student>(studentIndexKeysDefinition, indexOptions));
            LecturerCollection.Indexes.CreateOne(new CreateIndexModel<Lecturer>(lecturerIndexKeysDefinition, indexOptions));
            AdminCollection.Indexes.CreateOne(new CreateIndexModel<Admin>(adminIndexKeysDefinition, indexOptions));

        }
    }
}
