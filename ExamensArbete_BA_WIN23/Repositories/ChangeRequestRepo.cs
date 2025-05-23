﻿using ExamensArbete_BA_WIN23.API.Entities;
using ExamensArbete_BA_WIN23.Context;

namespace ExamensArbete_BA_WIN23.API.Repositories;

public class ChangeRequestRepo(ApplicationContext context) : Repo<ApplicationContext, ChangeRequest>(context), IChangeRequestRepo
{
    private readonly ApplicationContext _context = context;
}
