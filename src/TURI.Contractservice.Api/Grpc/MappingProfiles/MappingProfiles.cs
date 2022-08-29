using Application.Contracts.DTO;
using Application.JobOffer.Commands;
using AutoMapper;
using GrpcContract;
using System;

namespace TURI.Contractservice.Grpc.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<List<Application.Contracts.DTO.AvailableUnitsResult>, GrpcContract.AvailableUnitsResult>();
            CreateMap<Offer, CreateOfferCommand>();
                 
        }
    }
}
