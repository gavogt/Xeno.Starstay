using Microsoft.AspNetCore.Mvc.Rendering;

namespace Xeno.Starstay.Data
{
    public static class SpeciesCatalog
    {
        public static readonly string[] SpeciesOptions =
        {
            "Zeta Reticulan",
            "Nordic Envoy",
            "Mantis Collective",
            "Time Traveler",
            "Pleiadian Voyager",
            "Grey Hybrid",
            "Reptilian Diplomat",
            "Vulcan Scholar",
            "Martian Colonist",
            "Asgard Wayfarer"
        };

        public static IReadOnlyList<SelectListItem> BuildOptions(string? selectedValue = null, string? placeholder = null)
        {
            var items = new List<SelectListItem>();

            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                items.Add(new SelectListItem(placeholder, string.Empty, string.IsNullOrWhiteSpace(selectedValue)));
            }

            items.AddRange(SpeciesOptions.Select(species =>
                new SelectListItem(species, species, string.Equals(species, selectedValue, StringComparison.Ordinal))));

            return items;
        }
    }
}
