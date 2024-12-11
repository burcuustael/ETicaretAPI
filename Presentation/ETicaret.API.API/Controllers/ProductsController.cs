using System.Net;
using ETicaret.Domain.Entities;
using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.API.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;


        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }
        
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            RemoveProductCommandResponse response = await _mediator.Send(new RemoveProductCommandRequest{Id = id});
            return Ok();
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
          List<(string fileName, string pathOrContainerName)> result=  await _storageService.UploadAsync("photo-images", Request.Form);

          Product product = await _productReadRepository.GetByIdAsync(id);
          await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
          {
              FileName = r.fileName,
              Path = r.pathOrContainerName,
              Storage = _storageService.StorageName,
              Products = new List<Product>() {product}
          }).ToList());

          await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
         
            return Ok(product.ProductImageFiles.Select(p => new
            {
               Path=$"{_configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImages(string id,string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            ProductImageFile productImageFile = product.ProductImageFiles
                .FirstOrDefault(p => p.Id == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        
    }
}
