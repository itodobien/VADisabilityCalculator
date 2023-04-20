using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace VADisabilityCalculator
{
    public partial class MainPage : ContentPage
    {
        private List<double> disabilityRatings = new List<double>();

        public MainPage()
        {
            InitializeComponent();
            foreach (var button in GetGridButtons())
            {
                button.Clicked += OnPercentageClicked;
            }
        }

        private IEnumerable<Button> GetGridButtons()
        {
            var grid = (Grid)FindByName("ButtonGrid");
            return grid.Children.OfType<Button>().Where(button => button.Text != "Clear");
        }

        private void OnPercentageClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var rating = double.Parse(button.Text.TrimEnd('%'));

            disabilityRatings.Add(rating);
            UpdateEnteredRatingsLabel();

            double combinedRating = CalculateCombinedDisabilityRating(disabilityRatings);
            CombinedRatingLabel.Text = $"The overall combined disability rating is: {combinedRating}%";
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            disabilityRatings.Clear();
            UpdateEnteredRatingsLabel();
            CombinedRatingLabel.Text = "";
            EnteredRatingsLabel.Text = "You have selected: ";
        }


        private void UpdateEnteredRatingsLabel()
        {
            var ratings = string.Join(", ", disabilityRatings);
            var formattedString = new FormattedString();
            formattedString.Spans.Add(new Span { Text = "You have selected: ", FontAttributes = FontAttributes.None });
            formattedString.Spans.Add(new Span { Text = ratings, FontAttributes = FontAttributes.Bold });
            EnteredRatingsLabel.FormattedText = formattedString;
        }


        private double CalculateCombinedDisabilityRating(List<double> disabilityRatings)
        {
            if (disabilityRatings.Count == 0)
            {
                return 0;
            }

            disabilityRatings.Sort((a, b) => b.CompareTo(a));

            double combinedRating = 100;

            foreach (double rating in disabilityRatings)
            {
                combinedRating *= 1 - (rating / 100);
            }

            combinedRating = 100 - combinedRating;
            combinedRating = Math.Round(combinedRating / 10) * 10;

            return combinedRating;
        }
    }
}
