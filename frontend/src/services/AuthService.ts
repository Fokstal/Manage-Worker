import user from "../types/user";

class AuthService {
  private url = 'http://localhost:5177/account/';
  private key = 'KeyToAdd99Key';

  public register = async ({login, email, password} : user) : Promise<any> => {
    const res = await fetch(`${this.url}sign-up/${this.key}`, {
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

    if (!res.ok) throw new Error(`Registration error (invalid data or user allready exist)`);

    return await res.json();
  }

  public login = async ({login, email, password} : user) : Promise<any> => {
    const res = await fetch(`${this.url}login`, {
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
    
    if (!res.ok) throw new Error(`Login error (check that your password and email are entered correctly)`);

    return await res.json();
  }
}

export default AuthService;