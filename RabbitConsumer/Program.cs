using Newtonsoft.Json;
using RabbitConsumer;
using System.Net.Http.Headers;
using System.Text;

Console.WriteLine("Application Started.!");

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

await ConsumeFilms(client);

static async Task ConsumeFilms(HttpClient client)
{
    Console.WriteLine("ConsumeFilms Started.!");

    var data = JsonConvert.SerializeObject(new { UserName = "user", Password = "123" });
    var response = await client.PostAsync("https://localhost:7065/api/Authorizes/Authorize", new StringContent(data, Encoding.UTF8, "application/json"));
    string token = response.Content.ReadAsStringAsync().Result.ToString().Replace("\"", "");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    Console.WriteLine("Getted Token.!");

    var json = await client.GetStringAsync("https://api.themoviedb.org/3/list/1?api_key=a449e2153250ba81d683b951828c817b&language=en-US");
    var parsedModel = JsonConvert.DeserializeObject<Root>(json);
    var films = new List<Film>();

    Console.WriteLine("Collected Films.!");

    parsedModel?.items.ForEach(item =>
    {
        var film = new Film
        {
            VoteAverage = Convert.ToInt32(item.vote_average),
            VoteCount = item.vote_count,
            OriginalLanguage = item.original_language,
            OriginalTitle = item.original_title,
            Overview = item.overview,
            Point = Convert.ToInt32(item.popularity),
            PosterPath = item.poster_path,
            ReleaseDate = Convert.ToDateTime(item.release_date),
            Title = item.title,
            Adult = item.adult,
            Note = item.overview
        };
        films.Add(film);
    });

    Console.WriteLine("Sending Films.!");

    var addFilmData = JsonConvert.SerializeObject(new { Films = films });
    var addFilmResponse = await client.PostAsync("https://localhost:7065/api/Films/AddFilm", new StringContent(addFilmData, Encoding.UTF8, "application/json"));
    string addFilmResponseContent = response.Content.ReadAsStringAsync().Result;

    Console.WriteLine("ConsumeFilms End.!");
}


Console.WriteLine("Application End.!");
