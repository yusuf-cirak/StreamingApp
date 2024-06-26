﻿global using MediatR;
global using SharedKernel;
global using Domain.Entities;
global using Domain.Errors;
global using Domain.Constants;
global using Application.Abstractions.Repository;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Http;
global using Application.Abstractions.Helpers;
global using Application.Abstractions.Security;
global using System.Security.Claims;
global using Application.Contracts.OperationClaims;
global using Application.Contracts.Auths;
global using Application.Contracts.RoleOperationClaims;
global using Application.Contracts.Roles;
global using Application.Contracts.StreamOptions;
global using Application.Contracts.Streams;
global using Application.Contracts.Users;