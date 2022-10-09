using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeatherManager : SingletonMonobehaviour<WeatherManager>
{
    private Weather currentWeather = Weather.sunny;
    Weather[] dailyWeatherStates = new Weather[24];

    private void Awake()
    {
        SetDailyWeather();
        SetCurrentWeather();
    }

    private new WeatherChance[] weatherChanceList = new WeatherChance[]
    {
        new WeatherChance
        (
             Season.Winter,
            new WeatherStateChance[] {
                new WeatherStateChance (Weather.raining, 25 ),
                new WeatherStateChance ( Weather.snowing,  50),
                new WeatherStateChance (Weather.sunny, 25 )
            }
        ),

        new WeatherChance
        (
            Season.Spring,
            new WeatherStateChance[] {
                new WeatherStateChance (Weather.raining, 35),
                new WeatherStateChance (Weather.snowing, 5 ),
                new WeatherStateChance (Weather.sunny, 60 )
            }
        ),
        new WeatherChance
        (
            Season.Summer,
            new WeatherStateChance[] {
                new WeatherStateChance (Weather.raining, 10 ),
                new WeatherStateChance (Weather.snowing, 0 ),
                new WeatherStateChance (Weather.sunny, 90 )
            }
        ),
        new WeatherChance
        (
            Season.Autumn,
            new WeatherStateChance[] {
                new WeatherStateChance (Weather.raining, 60 ),
                new WeatherStateChance (Weather.snowing, 5 ),
                new WeatherStateChance (Weather.sunny, 35 )
            }
        )
    };

    internal class WeatherChance
    {
        public Season SeasonType {get; set; }
        public WeatherStateChance[] WeatherChanceList { get; set; }

        public WeatherChance(Season seasonType, WeatherStateChance[] weatherChanceList)
        {
            SeasonType = seasonType;
            WeatherChanceList = weatherChanceList;
        }
    }

    internal class WeatherStateChance
    {
        public Weather WeatherState { get; set; }
        public int Chance { get; set; }

        public WeatherStateChance(Weather weatherState, int chance)
        {
            WeatherState = weatherState;
            Chance = chance;
        }
    }

    public void SetDailyWeather()
    {

        WeatherChance currentWeatherChances = weatherChanceList.FirstOrDefault(chanceSeason => chanceSeason.SeasonType == TimeManager.Instance.GetGameSeason());  //weatherChanceList.Find(chance => chance.season == randomSeason).chanceList;
        var newChances = currentWeatherChances.WeatherChanceList.OrderByDescending(chance => chance.Chance).ToList();

        for (int i = 0; i < 8; i++)
        {
            int perCent = Random.Range(0, 100);

            Weather weatherState = Weather.sunny;

            if (perCent < newChances[0].Chance)
            {
                weatherState = newChances[0].WeatherState;
            }
            else if (perCent < newChances[0].Chance + newChances[1].Chance)
            {
                weatherState = newChances[1].WeatherState;
            }
            else if (perCent < newChances[0].Chance + newChances[1].Chance + newChances[2].Chance)
            {
                weatherState = newChances[2].WeatherState;
            }

            for (int j = 0; j < 3; j++)
            {
                dailyWeatherStates[3 * i + j] = weatherState;
            }
        }
        Debug.Log("******************************LÝSTE:*************************************");
        foreach (Weather weather in dailyWeatherStates)
        {
            Debug.Log("Liste Elemaný: " + weather);
        }
    }

    public void SetCurrentWeather()
    {
        int currentHour = TimeManager.Instance.GetGameHour();
        int hourIndex = currentHour / 3;
        this.currentWeather = dailyWeatherStates[hourIndex];
        rain.gameObject.SetActive(false);
        snow.gameObject.SetActive(false);
        snow.Stop();
        rain.Stop();
        switch (this.currentWeather)
        {
            case Weather.raining:
                rain.gameObject.SetActive(true);
                rain.Play();
                break;
            case Weather.snowing:
                snow.gameObject.SetActive(true);
                snow.Play();
                break;
            case Weather.sunny:
                break;
            default:
                break;
        }


        GameManager.Instance.currentWeather = this.currentWeather;

        Debug.Log("Current Weather: " + currentWeather  + "Hour Index: " + hourIndex);
    }



    //private List<object> weatherChanceList = new List<object>()
    //{
    //            new {
    //                    SeasonName = Season.Winter,
    //                    WeatherChance =  new List<object>() {
    //                                   new { WeatherCondition = Weather.raining, value = 20},
    //                                   new { WeatherCondition = Weather.sunny, value = 20},
    //                                   new { WeatherCondition = Weather.snowing, value = 60},
    //                    }
    //                },
    //            new {
    //                    SeasonName = Season.Spring,
    //                    WeatherChance =  new List<object>() {
    //                                   new { WeatherCondition = Weather.raining, value = 40},
    //                                   new { WeatherCondition = Weather.sunny, value = 55},
    //                                   new { WeatherCondition = Weather.snowing, value = 5},
    //                    }
    //                },
    //            new {
    //                    SeasonName = Season.Summer,
    //                    WeatherChance =  new List<object>() {
    //                                   new { WeatherCondition = Weather.raining, value = 15},
    //                                   new { WeatherCondition = Weather.sunny, value = 85},
    //                                   new { WeatherCondition = Weather.snowing, value = 0},
    //                    }
    //                },
    //            new {
    //                    SeasonName = Season.Autumn,
    //                    WeatherChance =  new List<object>() {
    //                                   new { WeatherCondition = Weather.raining, value = 60},
    //                                   new { WeatherCondition = Weather.sunny, value = 35},
    //                                   new { WeatherCondition = Weather.snowing, value = 5},
    //                    }
    //                },
    //};

    public ParticleSystem rain;
    public ParticleSystem snow;
    
    //public void SetWeatherConditionsOfTheDay()
    //{
    //    Season currentSeason = TimeManager.Instance.GetGameSeason();
    //    object chance = weatherChanceList.Where( weatherChance => weatherChance == currentSeason).FirstOrDefault();
    //}

    public void ActivateParticles()
    {
        this.snow.Stop();
        this.rain.Stop();
        switch (this.currentWeather)
        {
            case Weather.raining:
                this.rain.Play();
                break;
            case Weather.snowing:
                this.snow.Play();
                break;
            case Weather.sunny:
                break;
            default:
                break;
        }
        /*
        if (weatherType != this.currentWeather)
        {
            this.currentWeather = weatherType;
            this.snow.Stop();
            this.rain.Stop();
            switch (seasonType)
            {
                case Season.Summer:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:

                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.Spring:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:
                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.Autumn:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:
                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.Winter:
                    switch (weatherType)
                    {
                        case Weather.raining:
                            this.rain.Play();
                            break;
                        case Weather.snowing:
                            this.snow.Play();
                            break;
                        case Weather.sunny:
                            break;
                        case Weather.none:
                            break;
                        case Weather.count:
                            break;
                    }
                    break;
                case Season.none:
                    break;
                case Season.count:
                    break;
                default:
                    break;
            }
        }
        */
            GameManager.Instance.currentWeather = this.currentWeather;
    }

    //public void ChangeWeatherAccordingToTheSeason()
    //{
    //    int weatherNumber = Generator.Instance.RandomNumber(0, 3);
    //    ChangeWeather(TimeManager.Instance.GetGameSeason(), (Weather)weatherNumber);
    //}

    private void Update()
    {
        //ChangeWeatherAccordingToTheSeason();
        //ActivateParticles();
    }

    private void OnDisable()
    {
        EventHandler.SetCurrentWeather -= SetCurrentWeather;
        EventHandler.SetDailyWeather -= SetDailyWeather;
    }


    private void OnEnable()
    {
        EventHandler.SetCurrentWeather += SetCurrentWeather;
        EventHandler.SetDailyWeather += SetDailyWeather;
    }
}
