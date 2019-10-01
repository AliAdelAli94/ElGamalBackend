using ElGamal.DAL.Entities;
using ElGamal.DAL.GenericRepository;

namespace ElGamal.DAL.UOF
{
    public interface IUnitOfWork
    {

        GenericRepository<Category> CategoryRepository { get; }

        GenericRepository<Comment> CommentRepository { get; }

        GenericRepository<Favourite> FavouriteRepository { get; }

        GenericRepository<Image> ImageRepository { get; }

        GenericRepository<OrderDetail> OrderDetailRepository { get; }

        GenericRepository<Order> OrderRepository { get; }

        GenericRepository<Product> ProductRepository { get; }

        GenericRepository<User> UserRepository { get; }


        void Save();

        void Dispose();
    }
}
