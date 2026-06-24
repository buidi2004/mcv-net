using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dapper;

namespace CinemaXNet.Controllers;

[Authorize(Roles = "admin,cinema_manager")]
[Route("admin/revenue-charts")]
public class AdminRevenueChartsController(IDbConnection db) : Controller
{
    [HttpGet("api/daily")]
    public async Task<IActionResult> GetDailyRevenue(string date)
    {
        var sql = @"
            SELECT 
                DATE(show_time) as Date, 
                SUM(price) as TotalRevenue, 
                COUNT(*) as TotalTickets
            FROM bookings 
            WHERE booking_status = 'completed'
              AND DATE(show_time) = @Date
            GROUP BY DATE(show_time)
        ";
        
        var data = await db.QueryAsync<dynamic>(sql, new { Date = date });
        return Ok(data);
    }

    [HttpGet("api/weekly")]
    public async Task<IActionResult> GetWeeklyRevenue(string startDate)
    {
        var sql = @"
            SELECT 
                DATE(show_time) as Date, 
                SUM(price) as TotalRevenue, 
                COUNT(*) as TotalTickets
            FROM bookings 
            WHERE booking_status = 'completed'
              AND DATE(show_time) >= @StartDate
              AND DATE(show_time) <= DATE(@StartDate, '+6 days')
            GROUP BY DATE(show_time)
            ORDER BY Date
        ";
        
        var data = await db.QueryAsync<dynamic>(sql, new { StartDate = startDate });
        return Ok(data);
    }

    [HttpGet("api/monthly")]
    public async Task<IActionResult> GetMonthlyRevenue(string year)
    {
        var sql = @"
            SELECT 
                strftime('%Y-%m', show_time) as Month, 
                SUM(price) as TotalRevenue, 
                COUNT(*) as TotalTickets
            FROM bookings 
            WHERE booking_status = 'completed'
              AND strftime('%Y', show_time) = @Year
            GROUP BY strftime('%Y-%m', show_time)
            ORDER BY Month
        ";
        
        var data = await db.QueryAsync<dynamic>(sql, new { Year = year });
        return Ok(data);
    }
}
