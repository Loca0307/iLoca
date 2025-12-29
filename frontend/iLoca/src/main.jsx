import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
// global styles kept minimal; app-specific css is imported by each app
import App from './App.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App/>
  </StrictMode>,
)
