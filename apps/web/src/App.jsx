import './App.css'
import CreateSaleForm from './components/CreateSaleForm'

function App() {
  return (
    <div className="app">
      <header className="app-header">
        <h1>BeSpoked Bikes - Sales Management</h1>
      </header>
      <main className="app-main">
        <CreateSaleForm />
      </main>
    </div>
  )
}

export default App
