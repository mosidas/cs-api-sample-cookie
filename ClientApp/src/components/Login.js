import React, { Component } from 'react';
import './Login.css';

export class Login extends Component {
  constructor(props) {
    super(props);
    this.state = {
      email: '',
      password: '',
    };
  }

  handleSubmit = async (e) => {
    e.preventDefault();

    const { email, password } = this.state;
    const requestOptions = {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    };

    try {
      const response = await fetch('api/account/login', requestOptions);
      const data = await response.json();
      console.log("data: ", data);
      // ログイン成功時の処理（トークンの保存やリダイレクトなど）をここで行う
    } catch (error) {
      console.error('ログインエラー:', error);
      // エラー処理（エラーメッセージの表示など）をここで行う
    }
  };

  handleChange = (e) => {
    this.setState({ [e.target.id]: e.target.value });
  };

  render() {
    const { email, password } = this.state;

    return (
      <div className="login-container">
        <h2>ログイン画面</h2>
        <form onSubmit={this.handleSubmit}>
          <div>
            <label htmlFor="email">メールアドレス：</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={this.handleChange}
              required
            />
          </div>
          <div>
            <label htmlFor="password">パスワード：</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={this.handleChange}
              required
            />
          </div>
          <button type="submit">ログイン</button>
        </form>
      </div>
    );
  }
}

