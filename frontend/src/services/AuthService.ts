import user from "../types/user";

class AuthService {
  private url = 'http://localhost:5177/AccountAPI';
  private key = 'KeyToAdd99Key';

  public register = async ({login, email, password} : user) : Promise<string> => {
    const res = await fetch(`${this.url}/${this.key}`, {
      method : 'POST',
      headers : {
        'Content-Type' : 'application/json' 
      },
      body : JSON.stringify({
        "login": login,
        "email": email,
        "password": password,
      })
    });

    if (!res.ok) throw new Error(`Error while register ${res.statusText}`);

    return await res.text();
  }

  public login = async ({login, email, password} : user) : Promise<any> => {
    const res = await fetch(`${this.url}/`, {
      method : 'POST',
      headers : {
        'Content-Type' : 'application/json' 
      },
      body : JSON.stringify({
        "login": login,
        "email": email,
        "password": password,
      })
    });

    if (!res.ok) throw new Error(`Error while login ${res.statusText}`);

    return await res.text();
  }
}

export default AuthService;