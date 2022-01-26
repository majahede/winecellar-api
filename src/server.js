import express from 'express'

/**
 * The main function of the application.
 */
const main = async () => {
  const port = process.env.PORT || 8080
  const app = express()

  app.get('/', (req, res) => {
    res.send('Hello World!')
  })

  app.listen(port, () => {
    console.log(`Server running now at http://localhost:${port}`)
    console.log('Press Ctrl-C to terminate...')
  })
}

main().catch(console.error)
