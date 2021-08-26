
using AutoMapper;
using Microservices.Services.ShoppingCartAPI.DbContexts;
using Microservices.Services.ShoppingCartAPI.Models;
using Microservices.Services.ShoppingCartAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.ShoppingCartAPI.Repository;
public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public CartRepository(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async ValueTask<bool> ClearCartAsync(string userId)
    {
        CartHeader? cartHeader = await context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
        if (cartHeader is not null)
        {
            IQueryable<CartDetail>? cartDetails = context.CartDetails.Where(cd => cd.CartHeaderId == cartHeader.CartHeaderId);
            context.CartDetails.RemoveRange(cartDetails);
            context.CartHeaders.Remove(cartHeader);
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<CartDto> CreateUpdateCartAsync(CartDto cart)
    {
        Cart? cartEntity = mapper.Map<Cart>(cart);
        Product? existingProduct = context.Products
            .FirstOrDefault(p => p.ProductId == cart.CartDetails.FirstOrDefault().ProductId);
        if (existingProduct is null)
        {
            await context.Products.AddAsync(cartEntity.CartDetails.FirstOrDefault().Product);
            await context.SaveChangesAsync();
        }
        CartHeader? existingCartHeader = await context.CartHeaders.AsNoTracking()
            .FirstOrDefaultAsync(ch => ch.UserId == cartEntity.CartHeader.UserId);
        if (existingCartHeader is null)
        {
            await context.CartHeaders.AddAsync(cartEntity.CartHeader);
            await context.SaveChangesAsync();
            cartEntity.CartDetails.FirstOrDefault().CartHeaderId = cartEntity.CartHeader.CartHeaderId;
            cartEntity.CartDetails.FirstOrDefault().Product = null;
            await context.CartDetails.AddAsync(cartEntity.CartDetails.FirstOrDefault());
            await context.SaveChangesAsync();
        }
        else
        {
            CartDetail? existingCartDetail = await context.CartDetails.AsNoTracking()
                .FirstOrDefaultAsync(cd => cd.ProductId == cartEntity.CartDetails.FirstOrDefault().ProductId
                    && cd.CartHeaderId == existingCartHeader.CartHeaderId);
            if (existingCartDetail is null)
            {
                cartEntity.CartDetails.FirstOrDefault().CartHeaderId = existingCartHeader.CartHeaderId;
                cartEntity.CartDetails.FirstOrDefault().Product = null;
                await context.CartDetails.AddAsync(cartEntity.CartDetails.FirstOrDefault());
                await context.SaveChangesAsync();

            }
            else
            {
                cartEntity.CartDetails.FirstOrDefault().Count += existingCartDetail.Count;
                context.CartDetails.Update(cartEntity.CartDetails.FirstOrDefault());
                await context.SaveChangesAsync();
            }
        }
        CartDto? result = mapper.Map<CartDto>(cartEntity);
        return result;
    }

    public async Task<CartDto> GetCartByUserIdAsync(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await context.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId)
        };
        cart.CartDetails = context.CartDetails.Include(cd => cd.Product)
            .Where(cd => cd.CartHeaderId == cart.CartHeader.CartHeaderId);
        return mapper.Map<CartDto>(cart);
    }

    public async ValueTask<bool> RemoveFromCartAsync(Guid cartDetailId)
    {
        try
        {
            CartDetail? cartDetail = await context.CartDetails.FirstOrDefaultAsync(cd => cd.CartDetailId == cartDetailId);
            int totalCountOfCartItems = context.CartDetails.Count(cd => cd.CartHeaderId == cartDetail.CartHeaderId);
            context.CartDetails.Remove(cartDetail);
            if (totalCountOfCartItems == 1)
            {
                CartHeader? cartHeader = await context.CartHeaders.FirstOrDefaultAsync(ch => ch.CartHeaderId == cartDetail.CartHeaderId);
                context.CartHeaders.Remove(cartHeader);
            }
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
