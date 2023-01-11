using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;


        public AuthorsController(
            BookStoreDbContext context, 
            IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            try
            {
                var authors = await _context.Authors.ToListAsync();
                var authorsDto = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorsDto);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error performing Get in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogInformation($"Record not found: {nameof(GetAuthor)} - ID: {id}");
                    return NotFound();
                }

                var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = CustomRoleTypes.Administrator)]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                _logger.LogWarning($"Update Id invalid in {nameof(PutAuthor)} - Id: {id}");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            if(author == null)
            {
                _logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - Id: {id}");
                return NotFound(); 
            }
            
            _mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AuthorExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Error performing GET in {nameof(PutAuthor)}");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = CustomRoleTypes.Administrator)]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await _context.Authors.AddAsync(author); 
                await _context.SaveChangesAsync();
            
                return CreatedAtAction(nameof(GetAuthor), 
                    new { id = author.Id }, author);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error performing POST in {nameof(PostAuthor)}", authorDto);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = CustomRoleTypes.Administrator)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {

            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - Id: {id}");
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error performing DELETE in {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.Error500Message);

            }
        }

        private async Task<bool> AuthorExistsAsync(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
