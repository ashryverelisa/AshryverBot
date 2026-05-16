using AshryverBot.Database.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AshryverBot.Web.Components.Dialogs;

public partial class CommandEditDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CommandEntity Command { get; set; } = null!;
    [Parameter] public bool IsNew { get; set; }

    private MudForm _form = null!;
    private CommandEntity _model = null!;
    private bool _saving;

    protected override void OnInitialized()
    {
        _model = new CommandEntity
        {
            Id = Command.Id,
            Name = Command.Name,
            Response = Command.Response,
            IsEnabled = Command.IsEnabled,
            CooldownSeconds = Command.CooldownSeconds,
            RequiredRole = Command.RequiredRole,
            UsageCount = Command.UsageCount,
            CreatedAt = Command.CreatedAt,
            UpdatedAt = Command.UpdatedAt,
        };
    }

    private async Task Submit()
    {
        _saving = true;
        try
        {
            await _form.ValidateAsync();
            if (!_form.IsValid) return;
            MudDialog.Close(DialogResult.Ok(_model));
        }
        finally
        {
            _saving = false;
        }
    }

    private void Cancel() => MudDialog.Cancel();
}