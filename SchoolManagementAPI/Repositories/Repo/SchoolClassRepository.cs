﻿using MongoDB.Bson;
using MongoDB.Driver;
using SchoolManagementAPI.Models.Abstracts;
using SchoolManagementAPI.Models.Entities;
using SchoolManagementAPI.Models.Enum;
using SchoolManagementAPI.Repositories.Interfaces;
using SchoolManagementAPI.RequestResponse.Request;
using SchoolManagementAPI.Services.Configs;

namespace SchoolManagementAPI.Repositories.Repo
{
    public class SchoolClassRepository : ISchoolClassRepository
    {
        private readonly IMongoCollection<SchoolClass> _schoolClassCollection;
        private readonly IMongoCollection<Student> _studentCollection;
        private readonly IMongoCollection<Lecturer> _lecturerCollection;
        private readonly IMongoCollection<Semester> _semesterCollection;
        private readonly SortDefinition<SchoolClass> _sortSchoolClass;

        public SchoolClassRepository(DatabaseConfig databaseConfig)
        {
            _schoolClassCollection = databaseConfig.SchoolClassCollection;
            _studentCollection = databaseConfig.StudentCollection;
            _lecturerCollection = databaseConfig.LecturerCollection;
            _semesterCollection = databaseConfig.SemesterCollection;
            _sortSchoolClass = Builders<SchoolClass>.Sort.Descending(s => s.ID);
        }

        public async Task Create(SchoolClass schoolClass)
        {
            await _schoolClassCollection.InsertOneAsync(schoolClass);

            var filterStudent = Builders<Student>.Filter.In(u=>u.ID, schoolClass.StudentLogs.Select(s => s.ID).ToList());
            var updateStudent = Builders<Student>.Update.Push(s=>s.Classes, schoolClass.ID);

            var filterLecturer = Builders<Lecturer>.Filter.Eq(u => u.ID, schoolClass.Lecturer?.ID);
            var updateLecturer = Builders<Lecturer>.Update.Push(s => s.Classes, schoolClass.ID);

            var filterSemester = Builders<Semester>.Filter.Eq(s => s.ID, schoolClass.SemesterId);
            var updateSemester = Builders<Semester>.Update.Push(s => s.ClassIds, schoolClass.ID);

            Task student = _studentCollection.UpdateManyAsync(filterStudent, updateStudent);
            Task lecturer = _lecturerCollection.UpdateOneAsync(filterLecturer, updateLecturer);
            Task semester = _semesterCollection.UpdateOneAsync(filterSemester,updateSemester);
            await Task.WhenAll(student, lecturer,semester);
        }

        public async Task<SchoolClass?> Delete(string id)
        {
            var result = await _schoolClassCollection.FindOneAndDeleteAsync(s=>s.ID== id);
            return result;
        }

        public async Task<IEnumerable<SchoolClass>> GetbyTextFilter(string textFilter)
        {
            var filter = BsonDocument.Parse(textFilter);
            return await _schoolClassCollection.Find(filter).Sort(_sortSchoolClass).ToListAsync();
        }

        public async Task<IEnumerable<SchoolClass>> GetfromIds(List<string> ids)
        {
            var filter = Builders<SchoolClass>.Filter.In(s=>s.ID, ids);
            return await _schoolClassCollection.Find(filter).Sort(_sortSchoolClass).ToListAsync();
        }

        public async Task<IEnumerable<SchoolClass>> GetManyRange(int start, int end)
        {
            var sort = Builders<SchoolClass>.Sort.Descending(s => s.ID);
            return await _schoolClassCollection.Find(_ => true).Sort(sort).Skip(start).Limit(start - end).Sort(_sortSchoolClass).ToListAsync();
        }

        public async Task<SchoolClass?> GetSingle(string id)
        {
            var filter = Builders<SchoolClass>.Filter.Eq(s => s.ID, id);
            return await _schoolClassCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatebyFilter(FilterDefinition<SchoolClass> filter, UpdateDefinition<SchoolClass> update,bool isMany)
        {
            try
            {
                if(!isMany)
                    return (await _schoolClassCollection.UpdateOneAsync(filter, update)).ModifiedCount > 0;
                else
                    return (await _schoolClassCollection.UpdateManyAsync(filter, update)).ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating document: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> UpdatebyInstance(SchoolClass schoolClass)
        {
            var updateResult = await _schoolClassCollection.ReplaceOneAsync(s => s.ID == schoolClass.ID, schoolClass);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateByParameters(string id,IEnumerable<UpdateParameter> parameters)
        {
            var filter = Builders<SchoolClass>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<SchoolClass>.Update;
            List<UpdateDefinition<SchoolClass>> subUpdates = new List<UpdateDefinition<SchoolClass>>();
            foreach (var parameter in parameters)
            {
                switch (parameter.option)
                {
                    case UpdateOption.set:
                        subUpdates.Add(Builders<SchoolClass>.Update.Set(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.push:
                        subUpdates.Add(Builders<SchoolClass>.Update.Push(parameter.fieldName, parameter.value));
                        break;
                    case UpdateOption.pull:
                        subUpdates.Add(Builders<SchoolClass>.Update.Pull(parameter.fieldName, parameter.value));
                        break;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);

            UpdateResult result = await _schoolClassCollection.UpdateOneAsync(filter, combinedUpdate);
            return result.ModifiedCount > 0;
        }
    }
}
