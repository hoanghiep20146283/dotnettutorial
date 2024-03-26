using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CourseManagement.Entities;
using CourseManagement;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Repositories
{
    public class AuthorRepository : IAuthorRepository, IDisposable
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        public void DeleteAuthor(int authorID)
        {
            Author author = _context.Authors.Find(authorID);
            _context.Authors.Remove(author);
        }

        public void UpdateAuthor(Author author)
        {
            _context.Entry(author).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Author GetAuthorByID(int authorId)
        {
            return _context.Authors.Find(authorId);
        }

        public void InsertAuthor(Author author)
        {
            _context.Authors.Add(author);
            Save();
        }

        public void DeleteAuthor(int authorId)
        {
            Author author = _context.Authors.Find(authorId);
            _context.Authors.Remove(author);
            Save();
        }
    }
}