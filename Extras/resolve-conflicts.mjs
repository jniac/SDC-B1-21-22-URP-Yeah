#!/usr/bin/env node

import fs from 'fs'
import path from 'path'

/**
 * 
 * @param {string} dir 
 * @param {RegExp} filter 
 */
async function* walk(dir, filter) {
  for await (const d of await fs.promises.opendir(dir)) {
      const entry = path.join(dir, d.name)
      if (d.isDirectory()) {
        yield* await walk(entry, filter)
      }
      else if (d.isFile()) {
        if (filter.test(entry)) {
          yield entry
        }
      }
  }
}

/**
 * @typedef  {'incoming' | 'current'} ResolveMode 
 */

/**
 * 
 * @param {string} entry
 * @param {string} data
 * @param {ResolveMode} mode
 */
const checkData = (entry, data, mode = 'incoming') => {
  const startLength = '<<<<<<< HEAD\n'.length
  const middelLength = '=======\n'.length
  const endLength = '>>>>>>> 77eae295b361c2f00534528b4ee2592b5b86fc4a\n'.length

  const useIncoming = mode === 'incoming'
  let start = -1
  let modified = false
  while ((start = data.indexOf('<<<<<<< HEAD', start)) !== -1) {
    const middle = data.indexOf('=======', start)
    const end = data.indexOf('>>>>>>>', middle)

    const before = data.slice(0, start)
    const current = data.slice(start + startLength, middle)
    const incoming = data.slice(middle + middelLength, end)
    const after = data.slice(end + endLength)

    data = before + (useIncoming ? incoming : current) + after
    modified = true

    start += 1
  }

  return { modified, data }
}

const main = async () => {
  const dryRun = process.argv.includes('--dry-run')
  const filter = /\.(mat|asset)$/
  let count = 0
  for await (const entry of walk('Assets', filter)) {
    try {
      const source = await fs.promises.readFile(entry, 'utf-8')
      /** @type {ResolveMode} */
      const mode = 'incoming'
      const { modified, data } = checkData(entry, source, mode)
      if (modified) {
        if (dryRun === false) {
          await fs.promises.writeFile(entry, data)
        }
        console.log(`resolved (with ${mode}): ${entry}`)
      }
    }
    catch (e) {
      console.log(e)
    }
    if (count++ > 1000) break
  }

  if (dryRun) {
    console.log(`Note: Source files has not been modified (--dry-run)`)
  }
}

main()