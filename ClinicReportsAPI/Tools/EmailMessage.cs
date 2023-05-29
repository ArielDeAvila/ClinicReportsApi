namespace ClinicReportsAPI.Tools;

public static class EmailMessage
{
    public static string BodyMessage(string url)
    {
        string message = $"<!DOCTYPE html><html lang='es-ES'><head> <meta charset='UTF-8'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <meta name='viewport' content='width=device-width, initial-scale=1.0'></head><body style='text-align: center; font-family: sans-serif; font-size: large; margin: 0;'> <header style='text-align: center; margin: 0; background-color: rgba(185, 185, 185, 0.397); height: 15%; display: grid;'> <h1>System Report</h1> </header> <main style='padding: 7% 0;'> <section style='padding: 0 10%;'> <article style='text-align: center;'> <h2>Confirma el correo electrónico para completar la creación de tu perfil</h2> </article> <article style='text-align: center;'> <p>Haz clic en el siguiente botón para confirmar que este correo electrónico es tuyo.</p></article> <article style='text-align: center;'> <div class='btn' style='background-color: rgb(11, 113, 217); width: 75%; height: 8vh; margin: auto; border-radius: 7px; display: grid; align-items: center;'> <a style='text-decoration: none; color: antiquewhite;' href='{url}' target='_blank' rel='noopener noreferrer'>Confirmar el correo electrónico</a> </div></article> </section> </main> <footer style='text-align: center; margin-top: 5%; background-color: rgba(185, 185, 185, 0.397); height: 15%; display: grid; align-items: center;'> <p>&#169; 2023 System Report.</p></footer></body></html>";
        
        return message;
    }
}
