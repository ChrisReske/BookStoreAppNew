using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Models.Book;
using Microsoft.AspNetCore.Authorization;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(
            BookStoreDbContext context,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            var booksDto = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(booksDto);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            var bookDetailsDto = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (bookDetailsDto == null)
            {
              return NotFound();
            }

            return bookDetailsDto;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookUpdateDto)
        {
            if (id != bookUpdateDto.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();
            }


            _mapper.Map(bookUpdateDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = CustomRoleTypes.Administrator)]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookCreateDto)
        {
            var book = _mapper.Map<Book>(bookCreateDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new {id = book.Id}, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = CustomRoleTypes.Administrator)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
