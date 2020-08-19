import React, { Component } from 'react';
import { Route } from 'react-router';
import { useCookies } from 'react-cookie';

import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Counter } from './components/Counter';
import { FetchData } from './components/FetchData';
import Register from './components/Register';
import { Login } from './components/Login';
import { CoreGame } from './components/CoreGame';
import LanguageSwitcher from './components/LanguageSwitcher';

import { IGlobalSettings } from  './interfaces/IGlobalSettings';

import { COOKIE_NAME } from './Constants';

// global variable for I18N, herp derp
export var globalSettings:IGlobalSettings =
{
  "language": "en"
}

const App = (props:any) =>
{
  const [cookies] = useCookies([COOKIE_NAME]);

  if (cookies.language !== undefined)
  {
    globalSettings.language = cookies.language;
  }

  return (
    <Layout>
      <LanguageSwitcher />
      <Route exact path='/' component={Home} />
      <Route path='/register' component={Register} />
      <Route path='/login' component={Login} />
      <Route path='/counter' component={Counter} />
      <Route path='/fetch-data' component={FetchData} />
      <Route path='/core-game' component={CoreGame} />
    </Layout>
  );
};

export default App;