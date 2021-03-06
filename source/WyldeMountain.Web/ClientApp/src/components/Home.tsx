import { globalSettings } from '../App';
import { isUserAuthenticated, getCurrentUserAndDungeonAsync } from '../helpers/CurrentUser';
import { IUser } from '../interfaces/IUser';
import { Typography } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import LocalizedStrings from 'react-localization';
import { Redirect } from 'react-router';
import { Link } from 'react-router-dom';

export const Home = () =>
{
  const [user, setUser] = useState<IUser | undefined>(undefined);
  const [fetchedUser, setFetchedUser] = useState(false);

  const languageStrings = new LocalizedStrings({
    "en": require('~/../../resources/components/Home-en.json'),
    "ar": require('~/../../resources/components/Home-ar.json')
  });
  languageStrings.setLanguage(globalSettings.language);

  useEffect(() => {
    const fetchUser = async () => {
      if (!isUserAuthenticated())
      {
        setFetchedUser(true); // don't keep fetching
      }
      else if (!fetchedUser)
      {
        setFetchedUser(true);
        var data = await getCurrentUserAndDungeonAsync();
        setUser(data);
      }
    }

    fetchUser();
  });

  if (isUserAuthenticated())
  {
    if (user?.dungeon != null)
    {
      return (
        <Redirect to="/explore" /> 
      );
    }

    return (
      <div>
        <h1>{languageStrings.formatString(languageStrings.welcome, { userName: user?.emailAddress })}</h1>
        <p>
          {languageStrings.awaitingAdventure} &nbsp;
          <Link to="/begin-adventure">{languageStrings.beginJourney}
          </Link>
        </p>
      </div>
    );
  }
  else
  {
    return (
      <div>
        <Typography variant="h2">Hello, world!</Typography>
        <p>Welcome to your new single-page application, built with:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
          <li><s><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</s></li>
          <li><a href='http://https://material-ui.com'>Material-UI</a> for layout and styling</li>
        </ul>
        <p>To help you get started, we have also set up:</p>
        <ul>
          <li><strong>Client-side navigation</strong>. For example, click <em>Counter</em> then <em>Back</em> to return here.</li>
          <li><strong>Development server integration</strong>. In development mode, the development server from <code>create-react-app</code> runs in the background automatically, so your client-side resources are dynamically built on demand and the page refreshes when you modify any file.</li>
          <li><strong>Efficient production builds</strong>. In production mode, development-time features are disabled, and your <code>dotnet publish</code> configuration produces minified, efficiently bundled JavaScript files.</li>
        </ul>
        <p>The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.</p>
      </div>
    );
  }
}
