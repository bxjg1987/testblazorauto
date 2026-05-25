// vite.config.ts
import Uni from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+plugin-uni@0.1.0_@dcloudio+vite-plugin-uni@3.0.0-4080520251106001_@vueuse+core@11_euflhklxvcn2by6nltuslqy2oi/node_modules/@uni-helper/plugin-uni/src/index.js";
import { isMpWeixin } from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+uni-env@0.2.2/node_modules/@uni-helper/uni-env/dist/index.mjs";
import UniHelperComponents from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+vite-plugin-uni-components@0.2.10_rollup@4.60.4/node_modules/@uni-helper/vite-plugin-uni-components/dist/index.mjs";
import UniHelperLayouts from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+vite-plugin-uni-layouts@0.1.11_rollup@4.60.4/node_modules/@uni-helper/vite-plugin-uni-layouts/dist/index.mjs";
import UniHelperManifest from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+vite-plugin-uni-manifest@0.2.12_vite@5.4.21_@types+node@20.19.41_sass@1.100.0_terser@5.48.0_/node_modules/@uni-helper/vite-plugin-uni-manifest/dist/index.mjs";
import UniHelperPages from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+vite-plugin-uni-pages@0.3.24_vite@5.4.21_@types+node@20.19.41_sass@1.100.0_terser@5.48.0_/node_modules/@uni-helper/vite-plugin-uni-pages/dist/index.mjs";
import Optimization from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-ku+bundle-optimizer@2.2.0_chokidar@3.6.0_vite@5.4.21_@types+node@20.19.41_sass@1.100.0_t_nfzcwoo4toyj74e5qz5sa54bo4/node_modules/@uni-ku/bundle-optimizer/dist/index.mjs";
import UniKuRoot from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-ku+root@1.4.1_vite@5.4.21_@types+node@20.19.41_sass@1.100.0_terser@5.48.0_/node_modules/@uni-ku/root/dist/index.mjs";
import { UniEchartsResolver } from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/uni-echarts@2.5.1_@emnapi+core@1.10.0_@emnapi+runtime@1.10.0_echarts@6.1.0_vue@3.4.38_typescript@5.5.4_/node_modules/uni-echarts/dist-resolver/index.mjs";
import { UniEcharts } from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/uni-echarts@2.5.1_@emnapi+core@1.10.0_@emnapi+runtime@1.10.0_echarts@6.1.0_vue@3.4.38_typescript@5.5.4_/node_modules/uni-echarts/dist-vite/index.mjs";
import UnoCSS from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/unocss@66.0.0_postcss@8.5.15_vite@5.4.21_@types+node@20.19.41_sass@1.100.0_terser@5.48.0__vue@3.4.38_typescript@5.5.4_/node_modules/unocss/dist/vite.mjs";
import AutoImport from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/unplugin-auto-import@0.18.6_@vueuse+core@11.3.0_vue@3.4.38_typescript@5.5.4___rollup@4.60.4/node_modules/unplugin-auto-import/dist/vite.js";
import { defineConfig, loadEnv } from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/vite@5.4.21_@types+node@20.19.41_sass@1.100.0_terser@5.48.0/node_modules/vite/dist/node/index.js";

// src/resolver.ts
import { kebabCase } from "file:///C:/Users/Think/.trae-cn/worktrees/abp/uniapp-v3-rewrite-spec-QRa3Rn/src/ZLJ.Admin.UniApp/node_modules/.pnpm/@uni-helper+vite-plugin-uni-components@0.2.10_rollup@4.60.4/node_modules/@uni-helper/vite-plugin-uni-components/dist/index.mjs";
function WotResolver() {
  return {
    type: "component",
    resolve: (name) => {
      if (name.match(/^Wd[A-Z]/)) {
        const compName = kebabCase(name);
        return {
          name,
          from: `@wot-ui/ui/components/${compName}/${compName}.vue`
        };
      }
    }
  };
}

// vite.config.ts
var vite_config_default = defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");
  return {
    base: "./",
    optimizeDeps: {
      exclude: ["@wot-ui/ui", "uni-echarts"]
    },
    server: {
      proxy: {
        "/api": {
          target: env.VITE_API_BASE_URL || "http://localhost:44315",
          changeOrigin: true
        }
      }
    },
    plugins: [
      UniHelperManifest(),
      UniHelperPages({
        dts: "src/uni-pages.d.ts",
        exclude: ["**/components/**/*.*"]
      }),
      UniHelperLayouts(),
      UniHelperComponents({
        resolvers: [WotResolver(), UniEchartsResolver()],
        dts: "src/components.d.ts",
        dirs: ["src/components", "src/business"],
        directoryAsNamespace: true
      }),
      UniKuRoot(),
      UniEcharts(),
      Uni(),
      Optimization({
        enable: isMpWeixin,
        logger: false
      }),
      AutoImport({
        imports: ["vue", "@vueuse/core", "pinia", "uni-app", {
          from: "@wot-ui/router",
          imports: ["createRouter", "useRouter", "useRoute"]
        }, {
          from: "@wot-ui/ui",
          imports: ["useToast", "useDialog", "useNotify", "CommonUtil"]
        }],
        dts: "src/auto-imports.d.ts",
        dirs: ["src/composables", "src/store", "src/utils"],
        vueTemplate: true
      }),
      UnoCSS()
    ],
    css: {
      preprocessorOptions: {
        scss: {
          api: "modern-compiler",
          silenceDeprecations: ["legacy-js-api"]
        }
      }
    }
  };
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcudHMiLCAic3JjL3Jlc29sdmVyLnRzIl0sCiAgInNvdXJjZXNDb250ZW50IjogWyJjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZGlybmFtZSA9IFwiQzpcXFxcVXNlcnNcXFxcVGhpbmtcXFxcLnRyYWUtY25cXFxcd29ya3RyZWVzXFxcXGFicFxcXFx1bmlhcHAtdjMtcmV3cml0ZS1zcGVjLVFSYTNSblxcXFxzcmNcXFxcWkxKLkFkbWluLlVuaUFwcFwiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9maWxlbmFtZSA9IFwiQzpcXFxcVXNlcnNcXFxcVGhpbmtcXFxcLnRyYWUtY25cXFxcd29ya3RyZWVzXFxcXGFicFxcXFx1bmlhcHAtdjMtcmV3cml0ZS1zcGVjLVFSYTNSblxcXFxzcmNcXFxcWkxKLkFkbWluLlVuaUFwcFxcXFx2aXRlLmNvbmZpZy50c1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vQzovVXNlcnMvVGhpbmsvLnRyYWUtY24vd29ya3RyZWVzL2FicC91bmlhcHAtdjMtcmV3cml0ZS1zcGVjLVFSYTNSbi9zcmMvWkxKLkFkbWluLlVuaUFwcC92aXRlLmNvbmZpZy50c1wiO2ltcG9ydCBVbmkgZnJvbSAnQHVuaS1oZWxwZXIvcGx1Z2luLXVuaSdcbmltcG9ydCB7IGlzTXBXZWl4aW4gfSBmcm9tICdAdW5pLWhlbHBlci91bmktZW52J1xuaW1wb3J0IFVuaUhlbHBlckNvbXBvbmVudHMgZnJvbSAnQHVuaS1oZWxwZXIvdml0ZS1wbHVnaW4tdW5pLWNvbXBvbmVudHMnXG5pbXBvcnQgVW5pSGVscGVyTGF5b3V0cyBmcm9tICdAdW5pLWhlbHBlci92aXRlLXBsdWdpbi11bmktbGF5b3V0cydcbmltcG9ydCBVbmlIZWxwZXJNYW5pZmVzdCBmcm9tICdAdW5pLWhlbHBlci92aXRlLXBsdWdpbi11bmktbWFuaWZlc3QnXG5pbXBvcnQgVW5pSGVscGVyUGFnZXMgZnJvbSAnQHVuaS1oZWxwZXIvdml0ZS1wbHVnaW4tdW5pLXBhZ2VzJ1xuaW1wb3J0IE9wdGltaXphdGlvbiBmcm9tICdAdW5pLWt1L2J1bmRsZS1vcHRpbWl6ZXInXG5pbXBvcnQgVW5pS3VSb290IGZyb20gJ0B1bmkta3Uvcm9vdCdcbmltcG9ydCB7IFVuaUVjaGFydHNSZXNvbHZlciB9IGZyb20gJ3VuaS1lY2hhcnRzL3Jlc29sdmVyJ1xuaW1wb3J0IHsgVW5pRWNoYXJ0cyB9IGZyb20gJ3VuaS1lY2hhcnRzL3ZpdGUnXG5pbXBvcnQgVW5vQ1NTIGZyb20gJ3Vub2Nzcy92aXRlJ1xuaW1wb3J0IEF1dG9JbXBvcnQgZnJvbSAndW5wbHVnaW4tYXV0by1pbXBvcnQvdml0ZSdcbmltcG9ydCB7IGRlZmluZUNvbmZpZywgbG9hZEVudiB9IGZyb20gJ3ZpdGUnXG5pbXBvcnQgeyBXb3RSZXNvbHZlciB9IGZyb20gJy4vc3JjL3Jlc29sdmVyJ1xuXG5leHBvcnQgZGVmYXVsdCBkZWZpbmVDb25maWcoKHsgbW9kZSB9KSA9PiB7XG4gIGNvbnN0IGVudiA9IGxvYWRFbnYobW9kZSwgcHJvY2Vzcy5jd2QoKSwgJycpXG4gIHJldHVybiB7XG4gICAgYmFzZTogJy4vJyxcbiAgICBvcHRpbWl6ZURlcHM6IHtcbiAgICAgIGV4Y2x1ZGU6IFsnQHdvdC11aS91aScsICd1bmktZWNoYXJ0cyddLFxuICAgIH0sXG4gICAgc2VydmVyOiB7XG4gICAgICBwcm94eToge1xuICAgICAgICAnL2FwaSc6IHtcbiAgICAgICAgICB0YXJnZXQ6IGVudi5WSVRFX0FQSV9CQVNFX1VSTCB8fCAnaHR0cDovL2xvY2FsaG9zdDo0NDMxNScsXG4gICAgICAgICAgY2hhbmdlT3JpZ2luOiB0cnVlLFxuICAgICAgICB9LFxuICAgICAgfSxcbiAgICB9LFxuICAgIHBsdWdpbnM6IFtcbiAgICAgIFVuaUhlbHBlck1hbmlmZXN0KCksXG4gICAgICBVbmlIZWxwZXJQYWdlcyh7XG4gICAgICAgIGR0czogJ3NyYy91bmktcGFnZXMuZC50cycsXG4gICAgICAgIGV4Y2x1ZGU6IFsnKiovY29tcG9uZW50cy8qKi8qLionXSxcbiAgICAgIH0pLFxuICAgICAgVW5pSGVscGVyTGF5b3V0cygpLFxuICAgICAgVW5pSGVscGVyQ29tcG9uZW50cyh7XG4gICAgICAgIHJlc29sdmVyczogW1dvdFJlc29sdmVyKCksIFVuaUVjaGFydHNSZXNvbHZlcigpXSxcbiAgICAgICAgZHRzOiAnc3JjL2NvbXBvbmVudHMuZC50cycsXG4gICAgICAgIGRpcnM6IFsnc3JjL2NvbXBvbmVudHMnLCAnc3JjL2J1c2luZXNzJ10sXG4gICAgICAgIGRpcmVjdG9yeUFzTmFtZXNwYWNlOiB0cnVlLFxuICAgICAgfSksXG4gICAgICBVbmlLdVJvb3QoKSxcbiAgICAgIFVuaUVjaGFydHMoKSxcbiAgICAgIFVuaSgpLFxuICAgICAgT3B0aW1pemF0aW9uKHtcbiAgICAgICAgZW5hYmxlOiBpc01wV2VpeGluLFxuICAgICAgICBsb2dnZXI6IGZhbHNlLFxuICAgICAgfSksXG4gICAgICBBdXRvSW1wb3J0KHtcbiAgICAgICAgaW1wb3J0czogWyd2dWUnLCAnQHZ1ZXVzZS9jb3JlJywgJ3BpbmlhJywgJ3VuaS1hcHAnLCB7XG4gICAgICAgICAgZnJvbTogJ0B3b3QtdWkvcm91dGVyJyxcbiAgICAgICAgICBpbXBvcnRzOiBbJ2NyZWF0ZVJvdXRlcicsICd1c2VSb3V0ZXInLCAndXNlUm91dGUnXSxcbiAgICAgICAgfSwge1xuICAgICAgICAgIGZyb206ICdAd290LXVpL3VpJyxcbiAgICAgICAgICBpbXBvcnRzOiBbJ3VzZVRvYXN0JywgJ3VzZURpYWxvZycsICd1c2VOb3RpZnknLCAnQ29tbW9uVXRpbCddLFxuICAgICAgICB9XSxcbiAgICAgICAgZHRzOiAnc3JjL2F1dG8taW1wb3J0cy5kLnRzJyxcbiAgICAgICAgZGlyczogWydzcmMvY29tcG9zYWJsZXMnLCAnc3JjL3N0b3JlJywgJ3NyYy91dGlscyddLFxuICAgICAgICB2dWVUZW1wbGF0ZTogdHJ1ZSxcbiAgICAgIH0pLFxuICAgICAgVW5vQ1NTKCksXG4gICAgXSxcbiAgICBjc3M6IHtcbiAgICAgIHByZXByb2Nlc3Nvck9wdGlvbnM6IHtcbiAgICAgICAgc2Nzczoge1xuICAgICAgICAgIGFwaTogJ21vZGVybi1jb21waWxlcicsXG4gICAgICAgICAgc2lsZW5jZURlcHJlY2F0aW9uczogWydsZWdhY3ktanMtYXBpJ10sXG4gICAgICAgIH0sXG4gICAgICB9LFxuICAgIH0sXG4gIH1cbn0pXG4iLCAiY29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2Rpcm5hbWUgPSBcIkM6XFxcXFVzZXJzXFxcXFRoaW5rXFxcXC50cmFlLWNuXFxcXHdvcmt0cmVlc1xcXFxhYnBcXFxcdW5pYXBwLXYzLXJld3JpdGUtc3BlYy1RUmEzUm5cXFxcc3JjXFxcXFpMSi5BZG1pbi5VbmlBcHBcXFxcc3JjXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxUaGlua1xcXFwudHJhZS1jblxcXFx3b3JrdHJlZXNcXFxcYWJwXFxcXHVuaWFwcC12My1yZXdyaXRlLXNwZWMtUVJhM1JuXFxcXHNyY1xcXFxaTEouQWRtaW4uVW5pQXBwXFxcXHNyY1xcXFxyZXNvbHZlci50c1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vQzovVXNlcnMvVGhpbmsvLnRyYWUtY24vd29ya3RyZWVzL2FicC91bmlhcHAtdjMtcmV3cml0ZS1zcGVjLVFSYTNSbi9zcmMvWkxKLkFkbWluLlVuaUFwcC9zcmMvcmVzb2x2ZXIudHNcIjtpbXBvcnQgdHlwZSB7IENvbXBvbmVudFJlc29sdmVyIH0gZnJvbSAnQHVuaS1oZWxwZXIvdml0ZS1wbHVnaW4tdW5pLWNvbXBvbmVudHMnXG5cbmltcG9ydCB7IGtlYmFiQ2FzZSB9IGZyb20gJ0B1bmktaGVscGVyL3ZpdGUtcGx1Z2luLXVuaS1jb21wb25lbnRzJ1xuXG4vKiogV290IFVJIFx1N0VDNFx1NEVGNlx1ODlFM1x1Njc5MFx1NTY2OFx1RkYwQ1x1NEY5QiB2aXRlLmNvbmZpZy50cyBcdTRFMkRcdTgxRUFcdTUyQThcdTYzMDlcdTk3MDBcdTZDRThcdTUxOEMgV290IFVJIFx1N0VDNFx1NEVGNiAqL1xuZXhwb3J0IGZ1bmN0aW9uIFdvdFJlc29sdmVyKCk6IENvbXBvbmVudFJlc29sdmVyIHtcbiAgcmV0dXJuIHtcbiAgICB0eXBlOiAnY29tcG9uZW50JyxcbiAgICByZXNvbHZlOiAobmFtZTogc3RyaW5nKSA9PiB7XG4gICAgICBpZiAobmFtZS5tYXRjaCgvXldkW0EtWl0vKSkge1xuICAgICAgICBjb25zdCBjb21wTmFtZSA9IGtlYmFiQ2FzZShuYW1lKVxuICAgICAgICByZXR1cm4ge1xuICAgICAgICAgIG5hbWUsXG4gICAgICAgICAgZnJvbTogYEB3b3QtdWkvdWkvY29tcG9uZW50cy8ke2NvbXBOYW1lfS8ke2NvbXBOYW1lfS52dWVgLFxuICAgICAgICB9XG4gICAgICB9XG4gICAgfSxcbiAgfVxufVxuIl0sCiAgIm1hcHBpbmdzIjogIjtBQUE0YyxPQUFPLFNBQVM7QUFDNWQsU0FBUyxrQkFBa0I7QUFDM0IsT0FBTyx5QkFBeUI7QUFDaEMsT0FBTyxzQkFBc0I7QUFDN0IsT0FBTyx1QkFBdUI7QUFDOUIsT0FBTyxvQkFBb0I7QUFDM0IsT0FBTyxrQkFBa0I7QUFDekIsT0FBTyxlQUFlO0FBQ3RCLFNBQVMsMEJBQTBCO0FBQ25DLFNBQVMsa0JBQWtCO0FBQzNCLE9BQU8sWUFBWTtBQUNuQixPQUFPLGdCQUFnQjtBQUN2QixTQUFTLGNBQWMsZUFBZTs7O0FDVnRDLFNBQVMsaUJBQWlCO0FBR25CLFNBQVMsY0FBaUM7QUFDL0MsU0FBTztBQUFBLElBQ0wsTUFBTTtBQUFBLElBQ04sU0FBUyxDQUFDLFNBQWlCO0FBQ3pCLFVBQUksS0FBSyxNQUFNLFVBQVUsR0FBRztBQUMxQixjQUFNLFdBQVcsVUFBVSxJQUFJO0FBQy9CLGVBQU87QUFBQSxVQUNMO0FBQUEsVUFDQSxNQUFNLHlCQUF5QixRQUFRLElBQUksUUFBUTtBQUFBLFFBQ3JEO0FBQUEsTUFDRjtBQUFBLElBQ0Y7QUFBQSxFQUNGO0FBQ0Y7OztBREhBLElBQU8sc0JBQVEsYUFBYSxDQUFDLEVBQUUsS0FBSyxNQUFNO0FBQ3hDLFFBQU0sTUFBTSxRQUFRLE1BQU0sUUFBUSxJQUFJLEdBQUcsRUFBRTtBQUMzQyxTQUFPO0FBQUEsSUFDTCxNQUFNO0FBQUEsSUFDTixjQUFjO0FBQUEsTUFDWixTQUFTLENBQUMsY0FBYyxhQUFhO0FBQUEsSUFDdkM7QUFBQSxJQUNBLFFBQVE7QUFBQSxNQUNOLE9BQU87QUFBQSxRQUNMLFFBQVE7QUFBQSxVQUNOLFFBQVEsSUFBSSxxQkFBcUI7QUFBQSxVQUNqQyxjQUFjO0FBQUEsUUFDaEI7QUFBQSxNQUNGO0FBQUEsSUFDRjtBQUFBLElBQ0EsU0FBUztBQUFBLE1BQ1Asa0JBQWtCO0FBQUEsTUFDbEIsZUFBZTtBQUFBLFFBQ2IsS0FBSztBQUFBLFFBQ0wsU0FBUyxDQUFDLHNCQUFzQjtBQUFBLE1BQ2xDLENBQUM7QUFBQSxNQUNELGlCQUFpQjtBQUFBLE1BQ2pCLG9CQUFvQjtBQUFBLFFBQ2xCLFdBQVcsQ0FBQyxZQUFZLEdBQUcsbUJBQW1CLENBQUM7QUFBQSxRQUMvQyxLQUFLO0FBQUEsUUFDTCxNQUFNLENBQUMsa0JBQWtCLGNBQWM7QUFBQSxRQUN2QyxzQkFBc0I7QUFBQSxNQUN4QixDQUFDO0FBQUEsTUFDRCxVQUFVO0FBQUEsTUFDVixXQUFXO0FBQUEsTUFDWCxJQUFJO0FBQUEsTUFDSixhQUFhO0FBQUEsUUFDWCxRQUFRO0FBQUEsUUFDUixRQUFRO0FBQUEsTUFDVixDQUFDO0FBQUEsTUFDRCxXQUFXO0FBQUEsUUFDVCxTQUFTLENBQUMsT0FBTyxnQkFBZ0IsU0FBUyxXQUFXO0FBQUEsVUFDbkQsTUFBTTtBQUFBLFVBQ04sU0FBUyxDQUFDLGdCQUFnQixhQUFhLFVBQVU7QUFBQSxRQUNuRCxHQUFHO0FBQUEsVUFDRCxNQUFNO0FBQUEsVUFDTixTQUFTLENBQUMsWUFBWSxhQUFhLGFBQWEsWUFBWTtBQUFBLFFBQzlELENBQUM7QUFBQSxRQUNELEtBQUs7QUFBQSxRQUNMLE1BQU0sQ0FBQyxtQkFBbUIsYUFBYSxXQUFXO0FBQUEsUUFDbEQsYUFBYTtBQUFBLE1BQ2YsQ0FBQztBQUFBLE1BQ0QsT0FBTztBQUFBLElBQ1Q7QUFBQSxJQUNBLEtBQUs7QUFBQSxNQUNILHFCQUFxQjtBQUFBLFFBQ25CLE1BQU07QUFBQSxVQUNKLEtBQUs7QUFBQSxVQUNMLHFCQUFxQixDQUFDLGVBQWU7QUFBQSxRQUN2QztBQUFBLE1BQ0Y7QUFBQSxJQUNGO0FBQUEsRUFDRjtBQUNGLENBQUM7IiwKICAibmFtZXMiOiBbXQp9Cg==
