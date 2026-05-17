using Discord;

namespace bot_discord_hubla.Application.Helpers;

internal static class CargoSincronizadorHelper
{
    private const string MentoradoRoleName = "Mentorado";

    internal static string? DeterminarTargetRole(IReadOnlyList<string> cargosAtivos, string vipRoleName)
    {
        if (cargosAtivos.Any(c => c.Equals(MentoradoRoleName, StringComparison.OrdinalIgnoreCase)))
            return MentoradoRoleName;

        if (cargosAtivos.Count > 0)
            return vipRoleName;

        return null;
    }

    internal static async Task SincronizarAsync(
        IReadOnlyCollection<IRole> guildRoles,
        IGuildUser guildUser,
        string? targetRole,
        string vipRoleName,
        ILogger logger)
    {
        var rolesGerenciadas = new[] { MentoradoRoleName, vipRoleName }.Distinct().ToList();

        foreach (var roleName in rolesGerenciadas)
        {
            var role = guildRoles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            if (role is null)
            {
                logger.LogWarning("Cargo '{RoleName}' não encontrado na guild.", roleName);
                continue;
            }

            if (targetRole is not null && roleName.Equals(targetRole, StringComparison.OrdinalIgnoreCase))
                await AdicionarCargoSeNecessarioAsync(guildUser, role, roleName, logger);
            else
                await RemoverCargoSeNecessarioAsync(guildUser, role, roleName, logger);
        }
    }

    private static async Task AdicionarCargoSeNecessarioAsync(
        IGuildUser guildUser, IRole role, string roleName, ILogger logger)
    {
        if (guildUser.RoleIds.Contains(role.Id))
            return;

        try
        {
            await guildUser.AddRoleAsync(role);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao adicionar cargo '{Role}' ao discordId={DiscordId}.", roleName, guildUser.Id);
        }
    }

    private static async Task RemoverCargoSeNecessarioAsync(
        IGuildUser guildUser, IRole role, string roleName, ILogger logger)
    {
        if (!guildUser.RoleIds.Contains(role.Id))
            return;

        try
        {
            await guildUser.RemoveRoleAsync(role);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao remover cargo '{Role}' do discordId={DiscordId}.", roleName, guildUser.Id);
        }
    }
}
