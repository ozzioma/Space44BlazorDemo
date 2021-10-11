using AutoMapper;

namespace Application.UseCases.Student
{
    public class StudentMapperProfile : Profile
    {
        public StudentMapperProfile()
        {
            CreateMap<Persistence.Student, StudentViewModel>().ReverseMap();
            CreateMap<Persistence.Student, StudentCreateCommand>().ReverseMap();
            CreateMap<Persistence.Student, StudentUpdateCommand>().ReverseMap();
        }
    }
}