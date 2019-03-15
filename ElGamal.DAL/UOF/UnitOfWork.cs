using System;
using ElGamal.DAL.GenericRepository;
using ElGamal.DAL.Entities;
using ElGamal.DAL.Context;

namespace ElGamal.DAL.UOF
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ElGamalContext context = new ElGamalContext();
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Comment> commentRepository;
        private GenericRepository<Favourite> favouriteRepository;
        private GenericRepository<Image> imageRepository;
        private GenericRepository<OrderDetail> orderDetailRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<ProductOption> productOptionRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<User> userRepository;


        private bool disposed = false;

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new GenericRepository<Category>(this.context);
                }

                return this.categoryRepository;
            }
        }

        public GenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new GenericRepository<Comment>(this.context);
                }

                return this.commentRepository;
            }
        }

        public GenericRepository<Favourite> FavouriteRepository
        {
            get
            {
                if (this.favouriteRepository == null)
                {
                    this.favouriteRepository = new GenericRepository<Favourite>(this.context);
                }

                return this.favouriteRepository;
            }
        }

        public GenericRepository<Image> ImageRepository
        {
            get
            {
                if (this.imageRepository == null)
                {
                    this.imageRepository = new GenericRepository<Image>(this.context);
                }

                return this.imageRepository;
            }
        }

        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                if (this.orderDetailRepository == null)
                {
                    this.orderDetailRepository = new GenericRepository<OrderDetail>(this.context);
                }

                return this.orderDetailRepository;
            }
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new GenericRepository<Order>(this.context);
                }
                return this.orderRepository;
            }
        }

        public GenericRepository<ProductOption> ProductOptionRepository
        {
            get
            {
                if (this.productOptionRepository == null)
                {
                    this.productOptionRepository = new GenericRepository<ProductOption>(this.context);
                }

                return this.productOptionRepository;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(this.context);
                }
                return this.productRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(this.context);
                }
                return this.userRepository;
            }
        }


        public void Save()
        {
               this.context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
