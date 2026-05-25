import type { ComponentResolver } from '@uni-helper/vite-plugin-uni-components'

import { kebabCase } from '@uni-helper/vite-plugin-uni-components'

/** Wot UI 组件解析器，供 vite.config.ts 中自动按需注册 Wot UI 组件 */
export function WotResolver(): ComponentResolver {
  return {
    type: 'component',
    resolve: (name: string) => {
      if (name.match(/^Wd[A-Z]/)) {
        const compName = kebabCase(name)
        return {
          name,
          from: `@wot-ui/ui/components/${compName}/${compName}.vue`,
        }
      }
    },
  }
}
