using AutoMapper;
using Finnisimo_Library_Backend.Application.Queries.Responses;
using Finnisimo_Library_Backend.Domain.Entities.Books;
using Finnisimo_Library_Backend.Domain.Entities.Loans;
using Finnisimo_Library_Backend.Domain.Entities.Reservations;
using Finnisimo_Library_Backend.Domain.Entities.Users;
using Finnisimo_Library_Backend.Domain.Entities.WishlistItem;

namespace Finnisimo_Library_Backend.Application.Core;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<LoanEntity, UserLoanResponse>()
        .ForMember(dest => dest.IdBook, opt => opt.MapFrom(src => src.Book.Id))
        .ForMember(dest => dest.Title,
                   opt => opt.MapFrom(src => src.Book.Title))
        .ForMember(dest => dest.Author,
                   opt => opt.MapFrom(src => src.Book.Author))
        .ForMember(dest => dest.CoverImageUrl,
                   opt => opt.MapFrom(src => src.Book.CoverImageUrl))
        .ForMember(dest => dest.NumberOfPages,
                   opt => opt.MapFrom(src => src.Book.NumberOfPages))
        .ForMember(dest => dest.Status,
                   opt => opt.MapFrom(src => src.Status.ToString()));

    CreateMap<ReservationEntity, UserReservationResponse>()
        .ForMember(dest => dest.BookTitle,
                   opt => opt.MapFrom(src => src.Book.Title))
        .ForMember(dest => dest.CoverImageUrl,
                   opt => opt.MapFrom(src => src.Book.CoverImageUrl));

    CreateMap<ReservationEntity, UserReservationResponse>()
        .ForMember(dest => dest.BookTitle,
                   opt => opt.MapFrom(src => src.Book.Title))
        .ForMember(dest => dest.CoverImageUrl,
                   opt => opt.MapFrom(src => src.Book.CoverImageUrl))
        .ForMember(dest => dest.Status,
                   opt => opt.MapFrom(src => src.Status.ToString()));

    CreateMap<WishlistItemEntity, UserWishlistResponse>()
        .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book.Id))
        .ForMember(dest => dest.Title,
                   opt => opt.MapFrom(src => src.Book.Title))
        .ForMember(dest => dest.Author,
                   opt => opt.MapFrom(src => src.Book.Author))
        .ForMember(dest => dest.CoverImageUrl,
                   opt => opt.MapFrom(src => src.Book.CoverImageUrl))
        .ForMember(dest => dest.AddedAt,
                   opt => opt.MapFrom(src => src.AddedAt));

    CreateMap<UserEntity, UserResponse>();
    CreateMap<BookEntity, BookResponse>();
    CreateMap<BookEntity, BookDetailResponse>();
  }
}
