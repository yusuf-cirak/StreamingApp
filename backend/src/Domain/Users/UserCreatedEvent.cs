﻿using MediatR;

namespace Domain.Events;
public readonly record struct UserCreatedEvent(User User) : IDomainEvent,INotification;
