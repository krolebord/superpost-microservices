import React from 'react'
import ReactDOM from 'react-dom/client'
import { myProfileQuery } from './api/users'
import { App } from './App'
import './index.css'
import { queryClient } from './query-client'

queryClient.prefetchQuery(myProfileQuery);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
)
